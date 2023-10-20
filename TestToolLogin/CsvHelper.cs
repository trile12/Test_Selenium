using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestToolLogin
{
    public class CsvHelper
    {
        static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string csvFilePath = Path.Combine(baseDirectory, "ListUserSendMessage.csv");

        public static List<User> ReadUserCSV()
        {

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var users = csv.GetRecords<User>().ToList();
                return users;
            };
        }

        public static void WriteUserCSV(List<User> listUsers)
        {
            using (var writer = new StreamWriter(csvFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(listUsers);
            }
        }
    }
}
