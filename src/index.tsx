import { NativeModules } from 'react-native';

type DnsLookupWindowsType = {
  multiply(a: number, b: number): Promise<number>;
};

const { DnsLookupWindows } = NativeModules;

export default DnsLookupWindows as DnsLookupWindowsType;
