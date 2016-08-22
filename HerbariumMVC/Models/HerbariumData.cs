using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HerbariumMVC.Models
{
    public class HerbariumData
    {

    }

    // ***** Define classes to match database tables

    public class Acquisition
    {
        public int AcquisitionID { get; set; }
        public int AcquisitionNo { get; set; }
        public string SpecimenGUID { get; set; }
        public string EthnobotanicalUse { get; set; }
        public DateTime CollectionDate { get; set; }
        public string CollectorNo { get; set; }
        public string Notes { get; set; }
        public string CollectorName { get; set; }
        public bool Deleted { get; set; }
        public int AIKey { get; set; }

        // UserName is to help keep track of the last entry a user made
        public string UserName { get; set; }
    }

    public class Image
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public DateTime ImageDate { get; set; }
        public string SpecimenGUID { get; set; }

        public int IIKey { get; set; }
    }


    public class Location
    {
        public string LocationGUID { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Elevation { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string LocationDesc { get; set; }
        public int LIKey { get; set; }
        public string Township { get; set; }
        public string Sect { get; set; }
        public string Range { get; set; }
    }



    
    public class Specimen
    {
        public string SpecimenGUID { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string SubSpecies { get; set; }
        public string Author { get; set; }
        public string SubAuthor { get; set; }
        public string LocationGUID { get; set; }
        public string Community { get; set; }
        public string SoilType { get; set; }
        public string SoilColor { get; set; }
        public string SoilMoisture { get; set; }
        public int SIKey { get; set; }
    }

    

    

    public class User
    {
        public String userName { get; set; }
        public String fName { get; set; }
        public String lName { get; set; }
        public String password { get; set; }
        public bool admin { get; set; }
    }
}