using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

/*
 * author: luislasonbra
 * date:
 *	2023/08/01
 */
namespace DualSenseController
{
	// ; comment text
	// [section]
	// name = Value
	// info ini
	// https://wiki.freepascal.org/Using_INI_Files/es#:~:text=En%20los%20archivos%20INI%2C%20se,permiten%20otros%20caracteres%20como%20(%23).
	public class IniFile
	{
		const string openSection = "[";
		const string closeSection = "]";
		const string comments = ";|#";
		const string commentKey = "comment";

		readonly string _text = "";
		readonly string _filename = "";
		readonly bool _createFile = false;
		readonly List<IniSection> _sections = new List<IniSection>();

		public static string rootIdentificationKey = generateRandomKey(6);

		private static string generateRandomKey(int maxLenKey)
		{
			string nKey = "";
			Random rand = new Random();
			const string encodeData = "0123456789abcdefghijklmnopqrstxyz";
			for (int i = 0; i < maxLenKey; i++)
			{
				nKey += encodeData[rand.Next(0, encodeData.Length - 1)];
			}
			return nKey;
		}

		public IniFile() : this(string.Empty, true) { }

		public IniFile(string filename, bool createFile = false)
		{
			_filename = filename;
			_createFile = createFile;
			if (!createFile)
			{
				_text = File.ReadAllText(filename); //, Encoding.UTF8
				parse(_text);
			}
			else _text = string.Empty;
		}

		public void Save() { Save(_filename); }

		public void Save(string filename)
		{
			string buffer = "";
			foreach (IniSection section in _sections)
			{
				if (section.name == rootIdentificationKey)
				{
					if (section.body.Count == 0) continue;
					foreach (IniValue value in section.body)
						buffer += value + "\n";
					buffer += "\n";
				}
				else buffer += section + "\n";
				//buffer += "\n";
			}
			buffer = buffer.Substring(0, buffer.Length - 1);
			//Debug.WriteLine(buffer);

			// delete file
			if (File.Exists(filename)) File.Delete(filename);

			// save to file
			using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.WriteLine(buffer);
				}
			}
		}

		public IniValue? getRootValue(string name)
		{
			foreach (IniSection? section in _sections)
			{
				if (section.name == rootIdentificationKey)
				{
					return section.getPropertie(name);
				}
			}
			return null;
		}
		public void setRootValue(string name, string value)
		{
			IniSection? rootSection = getRootSection();
			if (rootSection == null) _sections.Add(new IniSection(rootIdentificationKey));

			// update values
			for (int i = 0; i < _sections.Count; i++)
			{
				if (_sections[i].name == rootIdentificationKey)
				{
					_sections[i].setPropertie(name, value);
				}
			}
		}

		/// <summary>
		/// Gets Root section
		/// </summary>
		/// <returns></returns>
		public IniSection? getRootSection()
		{
			foreach (IniSection? section in _sections)
			{
				if (section.name == rootIdentificationKey)
				{
					return section;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the first match from the search
		/// </summary>
		/// <param name="name"></param>
		/// <returns>IniSection</returns>
		public IniSection? getSection(string name)
		{
			List<IniSection> sections = getSections(name);
			if (sections != null && sections.Count > 0) return sections[0];
			return null;
		}

		/// <summary>
		/// Gets all matches from the search
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public List<IniSection> getSections(string name)
		{
			List<IniSection> sections = new List<IniSection>();
			foreach (IniSection section in _sections)
			{
				if (section.name == name)
					sections.Add(section);
			}
			return sections;
		}

		public IniFile setSection(IniSection section)
		{
			for (int i = 0; i < _sections.Count; i++)
			{
				if (_sections[i].name == section.name)
				{
					_sections[i] = section;
					return this;
				}
			}
			_sections.Add(section);
			return this;
		}

		public IniFile addSection(IniSection section)
		{
			_sections.Add(section);
			return this;
		}

		/// <summary>
		/// Gets all sections
		/// </summary>
		/// <returns></returns>
		public List<IniSection> getSections() { return _sections; }


		#region Parse Method

		private bool isCommentLine(string line)
		{
			string[] data = comments.Split(new char[] { '|' });
			foreach (string comment in data)
			{
				if (line.StartsWith(comment))
				{
					return true;
				}
			}
			return false;
		}

		private IniValue parseComentary(string line) { return new IniValue(commentKey, line); }

		private void parse(string src)
		{
			int index = 0;
			List<string> lines = _fixedLines(src);
			IniSection rootSection = new IniSection(rootIdentificationKey);
			while (index < lines.Count)
			{
				string line = lines[index];
				// skipe commentary ';'
				if (isCommentLine(line) || string.IsNullOrWhiteSpace(line))
				{
					// is comment
					if (isCommentLine(line)) rootSection.body.Add(parseComentary(line));
					// next line
					index++;
				}
				// sections '[section_name]'
				else if (line.StartsWith(openSection)) index = parseSection(lines, index);
				// values 'key=value'
				else
				{
					rootSection.body.Add(parseValue(line));
					// next line
					index++;
				}
			}
			// add root section
			_sections.Insert(0, rootSection);
		}

		private IniValue parseValue(string line)
		{
			int len = line.IndexOf("=");
			return new IniValue(line.Substring(0, len), line.Substring(len + 1));
		}

		private int parseSection(List<string> lines, int index)
		{
			string line = lines[index];
			if (line.StartsWith(openSection))
			{
				// name section
				string name = line.Substring(1, line.IndexOf(closeSection) - 1);
				// next line
				index++;
				// new section
				IniSection section = new IniSection(name);
				// find body
				while (index < lines.Count)
				{
					line = lines[index];
					if (line == string.Empty) break;
					// skipe commentary ';'
					if (isCommentLine(line) || string.IsNullOrWhiteSpace(line))
					{
						// is commentary
						if (isCommentLine(line)) section.body.Add(parseComentary(line));
						// next line
						index++;
					}
					else
					{
						// add new value
						section.body.Add(parseValue(line));
						// next line
						index++;
					}
				}
				// add new section
				_sections.Add(section);
			}
			// return current line
			return index;
		}

		private string _skipeSpace(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				// removed white space on line
				if (!char.IsWhiteSpace(line[i]))
					return line.Substring(i);
			}
			return "";
		}

		private List<string> _fixedLines(string src)
		{
			// separate text to lines
			string[] lines = src.Split("\n");
			List<string> results = new List<string>();
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (!string.IsNullOrEmpty(line) && !char.IsWhiteSpace(line[0]))
				{
					if (i < lines.Length - 1)
					{
						//line = line.Substring(0, line.Length - 1);
					}
				}
				else line = "";

				//Debug.WriteLine("_fixedLines.line: |" + line + "|");

				// fixed line
				results.Add(_skipeSpace(line));
				//Debug.WriteLine(results[results.Count - 1]);
			}
			return results;
		}

		#endregion


		public class IniSection
		{
			public string name;
			public List<IniValue> body;

			//public IniSection() : this(string.Empty) { }
			public IniSection(string name) : this(name, new List<IniValue>()) { }
			public IniSection(string name, List<IniValue> body)
			{
				this.name = name;
				this.body = body;
			}

			public IniValue? this[string name] { get { return getPropertie(name); } }

			public string getValue(string name)
			{
				IniValue? prop = getPropertie(name);
				if (prop != null) return prop.value;
				return string.Empty;
			}

			public IniValue? getPropertie(string name)
			{
				foreach (IniValue value in body)
				{
					if (value.name == name)
					{
						return value;
					}
				}
				return null;
			}

			public IniSection setPropertie(string name, string value)
			{
				for (int i = 0; i < body.Count; i++)
				{
					if (body[i].name == name)
					{
						body[i] = new IniValue(name, value);
						return this;
					}
				}
				body.Add(new IniValue(name, value));
				return this;
			}

			public IniSection addPropertie(string name, string value)
			{
				body.Add(new IniValue(name, value));
				return this;
			}

			public override string ToString()
			{
				string buffer = openSection + name + closeSection + "\n";
				foreach (IniValue value in body)
				{
					buffer += value + "\n";
				}
				return buffer;//.Substring(0, buffer.Length - 1);
			}
		}

		public class IniValue
		{
			public string name;
			public string value;

			public IniValue() : this(string.Empty, string.Empty) { }

			public IniValue(string name, string value)
			{
				this.name = name;
				this.value = value;
			}

			public override string ToString()
			{
				if (!string.IsNullOrEmpty(name) && name == commentKey) return value;
				return name + "=" + value;
			}
		}
	}
}
