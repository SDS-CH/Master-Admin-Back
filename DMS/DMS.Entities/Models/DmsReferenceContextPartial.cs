using Microsoft.EntityFrameworkCore;

namespace DMS.Entities.Models;

public partial class DmsReferenceContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AAmortissement>()
            .Ignore(e => e.PcCompteComptable)
            .Ignore(e => e.PcCompteComptableNavigation);

        modelBuilder.Entity<TnDetailsPiece>()
            .Ignore(e => e.TnDetailsActionDetailPieceDestinationNavigations);

        modelBuilder.Entity<TnAgence>()
            .Ignore(e => e.DeviseEquivalenceNavigation)
            .Ignore(e => e.DeviseReferenceNavigation);

        modelBuilder.Entity<InvLocation>()
            .Ignore(e => e.InvStockMovementDestinationLocations)
            .Ignore(e => e.InvStockMovementSourceLocations);

        modelBuilder.Entity<TnTypesDossier>(entity =>
        {
            entity.Property(e => e.SensTrafic).HasMaxLength(50);
            entity.Property(e => e.ModeTransport).HasMaxLength(50);
        });
    }
}
