import { NativeModules } from 'react-native';

export type DnsClientType = {
  init: (options: DnsClientOptions) => Promise<void>;
  query: (domainName: string, queryType: QueryType) => Promise<any>;
};

export enum QueryType {
  //
  // Summary:
  //     A host address.
  A = 1,
  //
  // Summary:
  //     An authoritative name server.
  NS = 2,
  //
  // Summary:
  //     A mail destination (OBSOLETE - use MX).
  MD = 3,
  //
  // Summary:
  //     A mail forwarder (OBSOLETE - use MX).
  MF = 4,
  //
  // Summary:
  //     The canonical name for an alias.
  CNAME = 5,
  //
  // Summary:
  //     Marks the start of a zone of authority.
  SOA = 6,
  //
  // Summary:
  //     A mailbox domain name (EXPERIMENTAL).
  MB = 7,
  //
  // Summary:
  //     A mail group member (EXPERIMENTAL).
  MG = 8,
  //
  // Summary:
  //     A mailbox rename domain name (EXPERIMENTAL).
  MR = 9,
  //
  // Summary:
  //     A Null resource record (EXPERIMENTAL).
  NULL = 10,
  //
  // Summary:
  //     A well known service description.
  WKS = 11,
  //
  // Summary:
  //     A domain name pointer.
  PTR = 12,
  //
  // Summary:
  //     Host information.
  HINFO = 13,
  //
  // Summary:
  //     Mailbox or mail list information.
  MINFO = 14,
  //
  // Summary:
  //     Mail exchange.
  MX = 15,
  //
  // Summary:
  //     Text resources.
  TXT = 16,
  //
  // Summary:
  //     Responsible Person.
  RP = 17,
  //
  // Summary:
  //     AFS Data Base location.
  AFSDB = 18,
  //
  // Summary:
  //     An IPv6 host address.
  AAAA = 28,
  //
  // Summary:
  //     A resource record which specifies the location of the server(s) for a specific
  //     protocol and domain.
  SRV = 33,
  //
  // Summary:
  //     A SSH Fingerprint resource record.
  SSHFP = 44,
  //
  // Summary:
  //     RRSIG rfc3755.
  RRSIG = 46,
  //
  // Summary:
  //     DNS zone transfer request. This can be used only if DnsClient.DnsQuerySettings.UseTcpOnly
  //     is set to true as AXFR is only supported via TCP.
  //     The DNS Server might only return results for the request if the client connection/IP
  //     is allowed to do so.
  AXFR = 252,
  //
  // Summary:
  //     Generic any query *.
  ANY = 255,
  //
  // Summary:
  //     A Uniform Resource Identifier (URI) resource record.
  URI = 256,
  //
  // Summary:
  //     A certification authority authorization.
  CAA = 257,
}

export type DnsClientOptions = Record<string, unknown> & {
  IPAddresses?: string[] | null;
  IPEndpoints?: Array<{ Port: number; IPAddress: string }> | null;
};

const DnsClient = NativeModules.RNDnsClientModule as DnsClientType;
export default DnsClient;
