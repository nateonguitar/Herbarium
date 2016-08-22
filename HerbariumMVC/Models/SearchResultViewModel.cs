using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace HerbariumMVC.Models
{
    public class SearchResultViewModel
    {

        // Acquisition data
        public int AcquisitionNo { get; set; }
        public string EthnobotanicalUse { get; set; }

        [DisplayFormat(Data‌​FormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMod‌​e = true)]
        public DateTime CollectedDate { get; set; }
        public string CollectorNo { get; set; }
        public string Notes { get; set; }
        public string CollectorName { get; set; }
        public bool Deleted { get; set; }
        public int AIKey { get; set; }

        // UserName is to help keep track of the last entry a user made
        public string UserName { get; set; }




        // locationData
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


        public int IIKey { get; set; }
        public string ImageName { get; set; }

        [FileSize(10240)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase Image { get; set; }

        public bool MarkedForDeletion { get; set; }

    }
}
