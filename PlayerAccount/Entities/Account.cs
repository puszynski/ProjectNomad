using AccountModule.Entities.ValueObject;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccountModule.Entities
{
    internal class Account
    {
        //https://stackoverflow.com/questions/23081096/entity-framework-6-guid-as-primary-key-cannot-insert-the-value-null-into-column
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public GuestAccount? GuestAccount { get; set; }
        public RegisteredAccount? RegisteredAccount { get; set; }
    }
}
