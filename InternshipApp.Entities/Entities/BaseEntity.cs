using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp.Core.Entities
{
    public class BaseEntity<TKey>
    {
        [Key]
        public TKey? Id { get; set; }
    }
}
