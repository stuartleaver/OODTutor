using T802.Core.Domain.Students;

namespace T802.Data.Mapping.Students
{
    public class RewardPointsHistoryMap : T802EntityTypeConfiguration<RewardPointsHistory>
    {
        public RewardPointsHistoryMap()
        {
            this.ToTable("RewardPointsHistory");
            this.HasKey(rph => rph.Id);

            this.Property(rph => rph.UsedAmount).HasPrecision(18, 4);
            //this.Property(rph => rph.UsedAmountInCustomerCurrency).HasPrecision(18, 4);

            this.HasRequired(rph => rph.Student)
                .WithMany(c => c.RewardPointsHistory)
                .HasForeignKey(rph => rph.StudentId);

            //this.HasOptional(rph => rph.UsedWithOrder)
            //    .WithOptionalDependent(o => o.RedeemedRewardPointsEntry)
            //    .WillCascadeOnDelete(false);
        }
    }
}
