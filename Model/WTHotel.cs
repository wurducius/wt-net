using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindingTreeNet.Model
{
   public class WTHotel
    {
        public WTHotel()
        {
            description = new Description();
            description.address = new Address();
            description.location = new Location();
            description.contacts = new Dictionary<string, Contact>();
            description.images = new List<string>();
            description.amenities = new List<string>();
            description.roomTypes = new Dictionary<string, RoomType>();

            availability = new Availability();
            availability.latestSnapshot = new LatestSnapshot();
            availability.latestSnapshot.availability = new Dictionary<string, List<RoomtypeAvailability>>();

            ratePlans = new Dictionary<string, RatePlan>();
        }

        public Description description { get; set; }
        public Dictionary<string, RatePlan> ratePlans { get; set; }
        public Availability availability { get; set; }
        public string notifications { get; set; }
        public string booking { get; set; }


        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public class Occupancy
        {
            public int min { get; set; }
            public int max { get; set; }
        }

        public class Properties
        {
            public string nonSmoking { get; set; }
        }

        public class RoomType
        {
            public RoomType()
            {

                occupancy = new Occupancy();
                amenities = new List<string>();
                images = new List<string>();
                properties = new Properties();
            }

            public string name { get; set; }
            public string description { get; set; }
            public int totalQuantity { get; set; }
            public Occupancy occupancy { get; set; }
            public List<string> amenities { get; set; }
            public List<string> images { get; set; }
            public string updatedAt { get; set; }
            public Properties properties { get; set; }
        }


        //public class RoomTypes
        //{
        //    public AdditionalProp1 additionalProp1 { get; set; }
        //    public AdditionalProp2 additionalProp2 { get; set; }
        //    public AdditionalProp3 additionalProp3 { get; set; }
        //}

        public class AdditionalContact
        {
            public string title { get; set; }
            public string value { get; set; }
        }

        public class Contact
        {
            public string email { get; set; }
            public string phone { get; set; }
            public string url { get; set; }
            public string ethereum { get; set; }
            public List<AdditionalContact> additionalContacts { get; set; }
        }

        public class Address
        {
            public string line1 { get; set; }
            public string line2 { get; set; }
            public string postalCode { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
        }

        public class CancellationPolicy
        {
            public string from { get; set; }
            public string to { get; set; }
            public int deadline { get; set; }
            public int amount { get; set; }
        }

        public class Description
        {
            public Location location { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public Dictionary<string,RoomType> roomTypes { get; set; }
            public Dictionary<string, Contact> contacts { get; set; }
            public Address address { get; set; }
            public string timezone { get; set; }
            public string currency { get; set; }
            public List<string> images { get; set; }
            public List<string> amenities { get; set; }
            public string updatedAt { get; set; }
            public int defaultCancellationAmount { get; set; }
            public List<CancellationPolicy> cancellationPolicies { get; set; }
        }

        public class AvailableForReservation
        {
            public string from { get; set; }
            public string to { get; set; }
        }

        public class AvailableForTravel
        {
            public string from { get; set; }
            public string to { get; set; }
        }

        public class Conditions
        {
            public string from { get; set; }
            public string to { get; set; }
            public int minLengthOfStay { get; set; }
            public int maxAge { get; set; }
            public int minOccupants { get; set; }
        }

        public class Modifier
        {
            public double adjustment { get; set; }
            public Conditions conditions { get; set; }
        }

        public class BookingCutOff
        {
            public int min { get; set; }
            public int max { get; set; }
        }

        public class LengthOfStay
        {
            public int min { get; set; }
            public int max { get; set; }
        }

        public class Restrictions
        {
            public BookingCutOff bookingCutOff { get; set; }
            public LengthOfStay lengthOfStay { get; set; }
        }

        public class RatePlan
        {

            public RatePlan()
            {
                availableForReservation = new AvailableForReservation();
                availableForTravel = new AvailableForTravel();
                restrictions = new Restrictions();
                restrictions.bookingCutOff = new BookingCutOff();
                restrictions.lengthOfStay = new LengthOfStay();
                roomTypeIds = new List<string>(); 
            }
            public string name { get; set; }
            public string description { get; set; }
            public string currency { get; set; }
            public int price { get; set; }
            public List<string> roomTypeIds { get; set; }
            public string updatedAt { get; set; }
            public AvailableForReservation availableForReservation { get; set; }
            public AvailableForTravel availableForTravel { get; set; }
            public List<Modifier> modifiers { get; set; }
            public Restrictions restrictions { get; set; }
        }    

        public class AvailabilityRestrictions
        {
            public bool noArrival { get; set; }
            public bool noDeparture { get; set; }
        }

        public class RoomtypeAvailability
        {
            public RoomtypeAvailability()
            {
                restrictions = new AvailabilityRestrictions();
            }

            public string date { get; set; }
            public int quantity { get; set; }
            public AvailabilityRestrictions restrictions { get; set; }
        }

        public class LatestSnapshot
        {


            public string updatedAt { get; set; }
            public Dictionary<string, List<RoomtypeAvailability>> availability { get; set; }

        }

        public class Availability
        {
            public LatestSnapshot latestSnapshot { get; set; }
        }

    }
}
