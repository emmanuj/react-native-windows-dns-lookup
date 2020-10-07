using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative;

namespace RNDnsLookup
{
  public partial class ReactPackageProvider : IReactPackageProvider
  {
    public void CreatePackage(IReactPackageBuilder packageBuilder)
    {
      CreatePackageImplementation(packageBuilder);
    }

    /// <summary>
    /// This method is implemented by the C# code generator
    /// </summary>
    partial void CreatePackageImplementation(IReactPackageBuilder packageBuilder);
  }
}
