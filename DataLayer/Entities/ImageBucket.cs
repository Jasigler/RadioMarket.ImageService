using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities
{
    public class ImageBucket
    {
        [Key]
        public Guid bucket_id { get; set; }

        [Required]
        public int item_id { get; set; }
    }
}
