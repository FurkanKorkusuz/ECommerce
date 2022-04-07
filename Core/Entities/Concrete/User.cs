using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    /// <summary>
    /// User nesnem normalde Entities katmanında olmalıydı.
    /// Core katmanının amacı tüm katmanların erişebileceği ortak tool(araç) ları tutmak.
    /// Entities katmanı da nesnelerimin tutulduğu katman.
    /// Entities katmanı Core katmanından referans alır. Proje mimari yapısı bu şekilde planlanmıştır.
    /// Güvenlik ve Authentication işlemleri Core da yapılır. Burada Login için User entity sine ihtiyaç duyulur ancak .net kuralları gereği Entities katmannı Core dan referans alırken aynı anda Core da Entities den referans alamaz.
    /// Bu kuraldan dolayı User nesnesi bu katmanda tanımlanır. 

    /// Generic Repository de tablo adı dinamik olsun diye buradaki  [Table("Users")] Attribute ünden okuyorum.
    /// </summary>
    [Table("Users")]
    public class User : IEntity
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
    }
}
