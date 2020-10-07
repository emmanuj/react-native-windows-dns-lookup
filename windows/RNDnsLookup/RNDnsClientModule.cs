using DnsClient;
using Microsoft.ReactNative.Managed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RNDnsLookup
{
  class NameServerEndpoint
  {
    public string IPAddress { get; set; }
    public int Port { get; set; }
  }

  class DnsClientOptions
  {
    public string[] IPAddresses { get; set; }
    public NameServerEndpoint[] IPEndpoints { get; set; }
    public bool UseCache { get; set; }
    public int Retries { get; set; }
  }

  [ReactModule]
  class RNDnsClientModule
  {
    private ILookupClient lookupClient = new LookupClient();
    private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings();

    public ILookupClient LookupClient { get => lookupClient; set => lookupClient = value; }

    public JsonSerializerSettings SerializerSettings => serializerSettings;

    public RNDnsClientModule()
    {
      serializerSettings.Converters.Add(new IPAddressConverter());
      serializerSettings.Converters.Add(new IPEndPointConverter());
      serializerSettings.Formatting = Formatting.Indented;
    }

    /**
     * Used on the client to initialize the DNS Lookup Client witt options.
     */
    [ReactMethod("init")]
    public async Task Init(DnsClientOptions options, ReactPromise<JSValue> promise)
    {
      await Task.Run(() =>
      {
        LookupClientOptions lookupClientOptions = new LookupClientOptions();
        if (options.IPAddresses != null)
        {
          var ipAddresses = options.IPAddresses.Select(addr =>
          {
            IPAddress.TryParse(addr, out IPAddress address);
            return address;
          }).ToArray();

          lookupClientOptions = new LookupClientOptions(ipAddresses);
        }
        else if (options.IPEndpoints != null)
        {
          var ipEndPoints = options.IPEndpoints.Select(namesvr =>
          {
            IPAddress.TryParse(namesvr.IPAddress, out IPAddress address);
            var endpoint = new IPEndPoint(address, namesvr.Port);
            return endpoint;
          }).ToArray();

          lookupClientOptions = new LookupClientOptions(ipEndPoints);
        }
        lookupClientOptions.UseCache = options.UseCache;
        lookupClientOptions.Retries = options.Retries;
        LookupClient = new LookupClient(lookupClientOptions);
        promise.Resolve(true);
      });
    }

    [ReactMethod("query")]
    public async Task<string> QueryAsync(string domainName, int queryType)
    {

      QueryType queryType1 = (QueryType)queryType;

      var result = await LookupClient.QueryAsync(domainName, queryType1);
      if (result.HasError)
      {
        throw new InvalidOperationException(result.ErrorMessage);
      }
      Debug.WriteLine($"Result:  {result}");
      var answers = result.Answers;


      switch (queryType1)
      {
        case QueryType.A:
          return ToJsonString(answers.ARecords().ToArray());
        case QueryType.CNAME:
          return ToJsonString(answers.CnameRecords().ToArray());
        case QueryType.TXT:
          return ToJsonString(answers.TxtRecords().ToArray());
        case QueryType.MX:
          return ToJsonString(answers.MxRecords().ToArray());
        case QueryType.SRV:
          return ToJsonString(answers.SrvRecords().ToArray());
        default:
          return ToJsonString(result.AllRecords);
      }
    }

    private string ToJsonString<T>(T obj)
    {
      return JsonConvert.SerializeObject(obj, SerializerSettings);
    }
  }

  // TODO: Use React-Native-Windows Serialization API instead?
  class IPEndPointConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(IPEndPoint));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      IPEndPoint ep = (IPEndPoint)value;
      JObject jo = new JObject();
      jo.Add("Address", JToken.FromObject(ep.Address, serializer));
      jo.Add("Port", ep.Port);
      jo.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      JObject jo = JObject.Load(reader);
      IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
      int port = (int)jo["Port"];
      return new IPEndPoint(address, port);
    }
  }

  class IPAddressConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(IPAddress));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return IPAddress.Parse((string)reader.Value);
    }
  }
}
