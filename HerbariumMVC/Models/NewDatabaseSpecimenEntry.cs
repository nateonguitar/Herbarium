using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace HerbariumMVC.Models
{
    public class NewDatabaseSpecimenEntry
    {
        // Acquisition data
        public int AcquisitionID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int AcquisitionNo { get; set; }
        public string EthnobotanicalUse { get; set; }

        [DataType(DataType.Date)]
        public DateTime CollectedDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string CollectorNo { get; set; }
        public string Notes { get; set; }
        public string CollectorName { get; set; }
        public bool Deleted { get; set; }
        public int AIKey { get; set; }

        // UserName is to help keep track of the last entry a user made
        public string UserName { get; set; }




        // locationData
        public string LocationGUID { get; set; }
        [Range(-double.MaxValue, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Lat { get; set; }

        [Range(-double.MaxValue, double.MaxValue, ErrorMessage = "Please enter valid doubleNumber")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Lng { get; set; }

        [Range(0, 40000, ErrorMessage = "Please enter a valid positive number")]
        public int Elevation { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string LocationDesc { get; set; }
        public int LIKey { get; set; }
        public string Township { get; set; }
        public string Sect { get; set; }
        public string Range { get; set; }




        // Specimen data
        public string SpecimenGUID { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string SubSpecies { get; set; }
        public string Author { get; set; }
        public string SubAuthor { get; set; }
        public string Community { get; set; }
        public string SoilType { get; set; }
        public string SoilColor { get; set; }
        public string SoilMoisture { get; set; }
        public int SIKey { get; set; }

        public int numberOfLabels { get; set; }

        [FileSize(10240)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase Image { get; set; }
    }
}
