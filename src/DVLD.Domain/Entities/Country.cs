namespace DVLD.Domain.Entities
{
    public class Country
    {
        public int? CountryID { get; }
        public string CountryName { get; }

        public Country(int? countryID, string countryName)
        {
            CountryID = countryID;
            CountryName = countryName;
        }
    }
}
