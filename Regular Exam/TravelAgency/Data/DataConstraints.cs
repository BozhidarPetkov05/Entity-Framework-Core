using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Data
{
    public static class DataConstraints
    {
        //Customer
        public const int CustomerFullNameMinLength = 4; 
        public const int CustomerFullNameMaxLength = 60;
        public const int CustomerEmailMinLength = 6;
        public const int CustomerEmailMaxLength = 50;
        public const int CustomerPhoneNumberLength = 13;
        public const string CustomerPhoneNumberRegex = @"\+\d{12}";

        //TourPackage
        public const int TourPackageNameMinLength = 2;
        public const int TourPackageNameMaxLength = 40;
        public const int TourPackageDescriptionMaxLength = 200;

        //Guide
        public const int GuideFullNameMinLength = 4;
        public const int GuideFullNameMaxLength = 60;
        public const int GuideLanguageMinValue = 0;
        public const int GuideLanguageMaxValue = 4;
    }
}
