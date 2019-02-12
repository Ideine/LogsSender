using System;
using System.Globalization;
using System.IO;
using Ideine.LogsSender.Interfaces;
using Newtonsoft.Json;

namespace Ideine.LogsSender.Internals
{
	internal class JsonLogWriter : IObjectWriter, IArrayWriter
	{
		private readonly bool _extraLine;
		private readonly StringWriter _textWriter = new StringWriter();
		private readonly JsonWriter _jsonWriter;

		public JsonLogWriter(Formatting formatting = Formatting.None, bool extraLine = false)
		{
			_extraLine = extraLine;

			_jsonWriter = new JsonTextWriter(_textWriter) {Formatting = formatting};
			_jsonWriter.WriteStartObject();
		}

		IArrayWriter IArrayWriter.WriteValue(string value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(double value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(long value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(bool value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(DateTime value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(DateTimeOffset value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteValue(TimeSpan value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value}");
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteValue({value.GetType().Name}: {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, string value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, double value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, long value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, bool value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, DateTime value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, DateTimeOffset value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value.ToString("o", CultureInfo.InvariantCulture));
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteProperty(string property, TimeSpan value)
		{
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteValue(value);
			System.Diagnostics.Debug.WriteLine($"WriteProperty({value.GetType().Name}: {property} => {value} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteObject(string property, Action<IObjectWriter> objectWriter)
		{
			System.Diagnostics.Debug.WriteLine($"WriteObject(Action): {property}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteStartObject();

			objectWriter?.Invoke(this);

			_jsonWriter.WriteEndObject();
			System.Diagnostics.Debug.WriteLine($"WriteObject(Action): {property} : END");
			return this;
		}

		IObjectWriter IObjectWriter.WriteArray(string property, Action<IArrayWriter> arrayWriter)
		{
			System.Diagnostics.Debug.WriteLine($"WriteArray(Action): {property}");
			_jsonWriter.WritePropertyName(property);
			_jsonWriter.WriteStartArray();

			arrayWriter?.Invoke(this);

			_jsonWriter.WriteEndArray();
			System.Diagnostics.Debug.WriteLine($"WriteArray(Action): {property} : END");
			return this;
		}

		IArrayWriter IArrayWriter.WriteObject(Action<IObjectWriter> objectWriter)
		{
			System.Diagnostics.Debug.WriteLine($"WriteObject(Action) in array");
			_jsonWriter.WriteStartObject();

			objectWriter?.Invoke(this);

			_jsonWriter.WriteEndObject();
			System.Diagnostics.Debug.WriteLine($"WriteObject(Action) in array : END");
			return this;
		}

		public override string ToString()
		{
			_jsonWriter.WriteEndObject();

			var result = _textWriter.ToString();

			if (_extraLine)
			{
				result += Environment.NewLine;
			}

			return result;
		}
	}
}