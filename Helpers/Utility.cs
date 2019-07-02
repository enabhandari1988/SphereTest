using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SphereTest.Helpers
{
    using System;
    using System.Text.RegularExpressions;

    public static class Utility
    {

		private static readonly Random Random = new Random();

        /// <summary>
        /// Get Specfic Field value from a Json string
        /// </summary>
        /// <param name="json">Json string</param>
        /// <param name="level">the field level, each field level seprated by "."</param>
        /// <returns>field value</returns>
        public static string GetFieldValueFromJsonString(string json, string level)
        {
            dynamic Res = JObject.Parse(json);
            string[] levels = level.Split('.');
            string fieldName;
            dynamic fieldValue = "";
            fieldValue = Res[levels[0]];
            if (levels.Count() == 1)
            {
                return fieldValue.ToString();
            }

            int flag = 0;
            for (int i = 1; i < levels.Length; i++)
            {
                flag = 0;
                fieldName = levels[i];


                if (fieldValue is JArray)
                {

                    JArray jarr = (JArray)fieldValue;
                    foreach (JObject content in jarr.Children<JObject>())
                    {
                        foreach (JProperty prop in content.Properties())
                        {
                            Console.WriteLine(prop.Type);
                            Console.WriteLine(prop.Name);
                            Console.WriteLine(prop.Value);
                            if (prop.Name.ToLower().Equals(fieldName.ToLower()))
                            {
                                fieldValue = prop.Value;
                                flag = 1;
                                break;
                            }

                        }

                        if (flag == 1)
                        {
                            break;
                        }
                    }
                }
                else if (fieldValue is JObject)
                {
                    fieldValue = fieldValue[fieldName];
                    if (fieldValue != null)
                    {
                        flag = 1;
                    }
                }

                if (flag == 0)
                {
                    //Assert.Fail("Failed to found the field " + fieldName);
                }
            }

            return fieldValue.ToString();
		}

        public static string GetFieldValuewithExactNameFromJsonString(string json, string ParentNode, string ChildNodename)
        {
            JObject data = JObject.Parse(json);
            var dd = data["Results"][0][ParentNode].First(x => (string)x["Name"] == ChildNodename)["Value"]["Value"];
            return dd.ToString();
        }


		public static string checkParentnodes(string response)

		{
			
			int count = 0;

			JObject data = JObject.Parse(response);

			count = data["body"]["businessPricePlans"]["subscriptionVolumes"].Count();

			return count.ToString();

		}

		public static string checkChildnodes(string response)

		{



			int count = 0;

			JObject data = JObject.Parse(response);

			count = data["body"]["businessPricePlans"]["subscriptionVolumes"].Count();

			int countsubperiods = 0;



			foreach (var subscriptionVolume in data["body"]["businessPricePlans"]["subscriptionVolumes"].Children())

			{
				foreach (JObject content in subscriptionVolume.Children<JObject>())
				{
					foreach (JProperty prop in content.Properties())
					{
						if (prop.Name.ToLower().Equals("subscriptionPeriod"))
						{
							countsubperiods++;
							break;
						}
					}
				}

				
			}



			return countsubperiods.ToString();



		}

	}
}