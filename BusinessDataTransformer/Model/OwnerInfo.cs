using System;
namespace BusinessDataTransformer.Model
{
    public class OwnerInfo
    {
        public string OwnerId { get; set; }
        public string LegalFormOfOwner { get; set; }
        public string CountryOfOwner { get; set; }
        public string OwnerCountrySign { get; set; }
        public string OwnerType { get; set; }
        public string OwnerShare { get; set; }
        public string IsValid { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }

        public bool Equals(OwnerInfo x, OwnerInfo y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return (x.OwnerId == y.OwnerId)
                && (x.LegalFormOfOwner == y.LegalFormOfOwner)
                && (x.CountryOfOwner == y.CountryOfOwner)
                && (x.OwnerShare == y.OwnerShare);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + OwnerId.GetHashCode();
                hash = hash * 23 + LegalFormOfOwner.GetHashCode();
                hash = hash * 23 + CountryOfOwner.GetHashCode();
                return hash;
            }
        }

        public override string ToString() => $"{OwnerId} - {CountryOfOwner} from {FromTime:dd.MM.yyyy} to {ToTime:dd.MM.yyyy}";
    }
}
