using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace HotelBooking.UnitTests {
	public class JsonDataAttribute : DataAttribute {
		private readonly string _baseFilePath = "../../../";
		private string _filePath;

		public JsonDataAttribute(string filePath) {
			_filePath = filePath;
		}

		public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
			var fileData = File.ReadAllText(_baseFilePath + _filePath);
			return JsonConvert.DeserializeObject<List<object[]>>(fileData);
		}
	}
}
