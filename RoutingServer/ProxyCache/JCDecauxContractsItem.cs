using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProxyCache
{
    public class JCDecauxContractsItem : AbstractCacheItem<List<Contract>>
    {
        private static readonly String ApiUrl = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=ac8828262f441bebc10d8f59f3aa109399f6f66b";

        public JCDecauxContractsItem(String useless) : base(ApiUrl)
        {
            Console.WriteLine("Sending a request to obtain all contracts");
        }
    }

    [DataContract]
    public class Contract
    {
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public String commercial_name { get; set; }
        [DataMember]
        public String country_code { get; set; }
        [DataMember]
        public List<string> cities { get; set; }
    }
}
