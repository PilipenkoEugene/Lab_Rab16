using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Lab16
{
    class DataStorage : DataInterface
    {
        public bool IsReady
        {
            get
            {
                if (rawdata == null) return false;
                else return true;
            }
        }

        private List<RawDataItem> rawdata;
        private List<SummaryDataItem> sumdata;
        private char devider = '*';
        public DataStorage() { }

        private void BuildSummary()
        {
            Dictionary<int, float> tmp = new Dictionary<int, float>();
            int iterBus = 0;
            double muchCost = -1, airCost = 0;

            foreach (var item in rawdata)
            {
                if (item.Type == "Автобус")
                {
                    iterBus++;
                    muchCost = Math.Max(muchCost, item.Cost);
                }
                else if (item.Type == "Самолет")
                {
                    airCost += item.Cost;
                    muchCost = Math.Max(muchCost, item.Cost);
                }
            }

            sumdata = new List<SummaryDataItem>();
            sumdata.Add(new SummaryDataItem()
            {
                GroupName = "Суммарная стоимость билетов на самолет",
                GroupValue = airCost
            });
            sumdata.Add(new SummaryDataItem()
            {
                GroupName = "Количество автобусных рейсов",
                GroupValue = iterBus
            });
            sumdata.Add(new SummaryDataItem()
            {
                GroupName = "Самый дорогой билет",
                GroupValue = muchCost
            });
        }
        private bool InitData(String datapath)
        {
            rawdata = new List<RawDataItem>();

            try
            {
                StreamReader sr = new StreamReader(datapath);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(devider);
                    var item = new RawDataItem()
                    {
                        Type = items[0].Trim(),
                        From = items[1].Trim(),
                        To = items[2].Trim(),
                        Cost = Convert.ToSingle(items[3].Trim(), CultureInfo.InvariantCulture),
                    };
                    rawdata.Add(item);
                }
                sr.Close();
                BuildSummary();
            } catch (IOException ex)
            {
                return false;
            }
            return true;
        }

        public static DataStorage DataCreator(String path)
        {
            DataStorage d = new DataStorage();
            if (d.InitData(path))
                return d;
            else
                return null;
        }

        public List<RawDataItem> GetRawData()
        {
            if (this.IsReady)
                return rawdata;
            else
                return null;
        }

        public List<SummaryDataItem> GetSummaryData()
        {
            if (this.IsReady)
                return sumdata;
            else
                return null;
        }
    }
}
