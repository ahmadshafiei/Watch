using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models.Enum;

namespace Watch.Models
{
    public class Watch
    {
        public Watch()
        {
            Images = new List<Image>();
            DateCreated = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsHeader { get; set; }
        public bool IsBoxed { get; set; }
        //Determines if user can suggest price on this particular watch
        public bool SuggestPrice { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RefNo { get; set; }
        public string Watch24Id { get; set; }
        public string CaseMaterial { get; set; }
        public string BraceletMaterial { get; set; }
        public string MainImagePath { get; set; }
        public Gender Gender { get; set; }
        public Movement? CounterMovement { get; set; }
        public Condition Condition { get; set; }
        public string Year { get; set; }
        public DateTime DateCreated { get; set; }
        public int? Brand_Id { get; set; }
        [ForeignKey("Brand_Id")]
        public Brand Brand { get; set; }
        public int? OwnerUser_Id { get; set; }
        [ForeignKey("OwnerUser_Id")]
        public User OwnerUser { get; set; }
        public List<Request> Requests { get; set; }
        public List<Image> Images { get; set; }
        public List<WatchBookmark> WatchBookmarks { get; set; }
        public List<SuggestPrice> SuggestedPrices { get; set; }

        #region [Insert-Watch]
        //THESE TWO PROPERTY USED IN InsertWatch
        [NotMapped]
        public byte[] MainImage { get; set; }
        [NotMapped]
        public List<byte[]> SubImages { get; set; }
        #endregion

        #region [Watch-Detail] 
        [NotMapped]
        public bool IsBookmarked { get; set; }
        [NotMapped]
        public List<Watch> SimilarWatches { get; set; }
        #endregion
    }
}
