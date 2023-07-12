using AllPeople;
using System.Collections;
using System.Linq;
namespace Datastor
{
    public class PeopleData : IPersone<People>, IEnumerable//מדובר ברשימה אז נשתמש באינורבל בשביל שתהליך ההוספה הסרה חיפוש ושינוי וכו יהיה יותר דימני
    {

        readonly DbService _dbService = DbService.Instance;//שמירת אנשים

        public void SetSale() => ManagerP.OnSetSale();
        public void EndSale() => ManagerP.OnEndSale();
        public People Add(People people)
        {
            if (people != null)
            {
                _dbService.peoples.Add(people);
                return people;
            }
            return null;
        }
        public People Delete(People person)
        {
            var removePerson = _dbService.peoples.FirstOrDefault(i => i == person);
            if (removePerson != null)
            {
                _dbService.peoples.Remove(removePerson);
                return removePerson;
            }
            return null;
        }
        public IQueryable<People> Get() => _dbService.peoples.AsQueryable();
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _dbService.peoples.Count; i++)
                if (_dbService.peoples[i] != null)
                    yield return _dbService.peoples[i];
        }
    }
}
