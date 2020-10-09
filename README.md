# react-native-windows-dns-lookup

DNS Lookup in react native

## Installation

```sh
npm install react-native-windows-dns-lookup
```

## Usage

```js
import DnsLookupWindows from "react-native-windows-dns-lookup";

// ...
await DnsLookupWindows.init(); // or DnsLookupWindows.init({IPAddresses: addresses});
const result = await DnsLookupWindows.query("www.google.com", QueryType.A);
```

## Contributing

See the [contributing guide](CONTRIBUTING.md) to learn how to contribute to the repository and the development workflow.

## License

MIT
