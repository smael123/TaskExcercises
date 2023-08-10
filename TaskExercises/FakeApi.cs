using System;
using System.Collections.Concurrent;

namespace TaskExercises
{
	public class FakeApi
	{
		private readonly string[] _peopleNames;
		private readonly Dictionary<string, int> _salaryDictionary;

		public FakeApi()
		{
			_peopleNames = new string[] { "Adam", "Eve", "John" };

			_salaryDictionary = new()
			{
				{ "Adam", 100 },
                { "Eve", 150 },
                { "John", 200 },
            };
        }

		public void GetSalaryByName(string name, ConcurrentBag<(string, int)> salaryList, int sleepTime = 0, bool invokeError = false)
		{
			_salaryDictionary.TryGetValue(name, out int salary);

			Thread.Sleep(TimeSpan.FromSeconds(sleepTime));

            if (invokeError)
            {
                throw new Exception($"{nameof(GetSalaryByName)} Error. Person Name: {name}");
            }

            salaryList.Add((name, salary));
		}
    }
}

