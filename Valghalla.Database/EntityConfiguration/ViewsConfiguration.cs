using Valghalla.Database.Entities.Views;
using Valghalla.Database.Schema.Views;
using Microsoft.EntityFrameworkCore;

namespace Valghalla.Database.EntityConfiguration
{
    internal static class ViewsConfiguration
    {
        internal static void Configure(ModelBuilder modelBuilder, bool isInMemory)
        {
            modelBuilder.Entity<ApplicationView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.Bid);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.ApplicationView);


                entity.Property(e => e.Bid)
                    .HasMaxLength(4);
                entity.Property(e => e.FormName).HasMaxLength(50);
                entity.Property(e => e.FormPostMotivation).IsUnicode(false);
                entity.Property(e => e.FormPostCreated).HasColumnType("datetime");
                entity.Property(e => e.FormPostStatus).HasMaxLength(50);
                entity.Property(e => e.FieldName).HasMaxLength(50);
                entity.Property(e => e.FieldType).HasMaxLength(50);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo).HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.PersonTypeId);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");
            });

            modelBuilder.Entity<StaffingView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.Description);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.StaffingView);

                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCo)
                    .HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId).HasMaxLength(36).IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.PublicDescription).HasMaxLength(50);
            });

            modelBuilder.Entity<BuildingView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.BuildingName);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.BuildingView);

                entity.Property(e => e.BuildingName).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingEmail).HasMaxLength(255);
                entity.Property(e => e.BuildingWebpage).HasMaxLength(255);
                entity.Property(e => e.BuildingId);
                entity.Property(e => e.BuildingInfoToContactPerson);
                entity.Property(e => e.BuildingInformerNeeded)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingCanExpand)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingDeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingDistrict).HasMaxLength(50);
                entity.Property(e => e.BuildingDistrictId);
                entity.Property(e => e.BuildingStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingStatusId).HasMaxLength(36).IsUnicode(false);
                entity.Property(e => e.BuildingPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.BuildingType).HasMaxLength(50);
                entity.Property(e => e.BuildingTypeId);
                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.Expr1).HasMaxLength(101);
                entity.Property(e => e.Expr2).HasMaxLength(103);
                entity.Property(e => e.ParishName).HasMaxLength(50);
                entity.Property(e => e.ParishNumber).HasMaxLength(50);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.RoomName).HasMaxLength(255);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo).HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonGenderId).HasMaxLength(36).IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId).HasMaxLength(36).IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.PersonTypeId);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.AssignmentFunction).HasMaxLength(50);
                entity.Property(e => e.AssignmentPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ElectoralDistrictNumber).HasMaxLength(50);
                entity.Property(e => e.ConstituencyName).HasMaxLength(50);
                entity.Property(e => e.ConstituencyNumber).HasMaxLength(50);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");

            });

            modelBuilder.Entity<GroupView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.GroupName);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.GroupView);

                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.Expr1).HasMaxLength(100);
                entity.Property(e => e.ParishName).HasMaxLength(103);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo)
                    .HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.AssignmentFunction).HasMaxLength(50);
                entity.Property(e => e.AssignmentPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ConstituencyName).HasMaxLength(101);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");

            });

            modelBuilder.Entity<CourseOccassionView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.CourseOccassionId);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.CourseOccassionView);

                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.ParishName).HasMaxLength(103);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.CourseOccassionAddress).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionDate).HasColumnType("datetime");
                entity.Property(e => e.CourseOccassionCourseDescription).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionCourseInfo).HasMaxLength(250);
                entity.Property(e => e.CourseOccassionRoom).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionHall).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionEndTime).HasPrecision(0);
                entity.Property(e => e.CourseOccassionStartTime).HasPrecision(0);
                entity.Property(e => e.TeacherAddress).HasMaxLength(255);
                entity.Property(e => e.TeacherLastName).HasMaxLength(255);
                entity.Property(e => e.TeacherEmail).HasMaxLength(255);
                entity.Property(e => e.TeacherFirstName).HasMaxLength(255);
                entity.Property(e => e.TeacherGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.TeacherGenderId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.TeacherMobilePhone).HasMaxLength(255);
                entity.Property(e => e.TeacherSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.TeacherPostalCode).HasMaxLength(255);
                entity.Property(e => e.TeacherPostalPlace).HasMaxLength(255);
                entity.Property(e => e.TeacherStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.TeacherStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.TeacherPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.TeacherType).HasMaxLength(50);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo)
                    .HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.AssignmentFunction).HasMaxLength(50);
                entity.Property(e => e.AssignmentPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ConstituencyName).HasMaxLength(101);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");

            });

            modelBuilder.Entity<RoomView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.RoomName);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.RoomView);

                entity.Property(e => e.BuildingName).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingEmail).HasMaxLength(255);
                entity.Property(e => e.BuildingWebpage).HasMaxLength(255);
                entity.Property(e => e.BuildingId);
                entity.Property(e => e.BuildingInformerNeeded)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingCanExpand)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingDeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingDistrict).HasMaxLength(50);
                entity.Property(e => e.BuildingDistrictId);
                entity.Property(e => e.BuildingStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.BuildingType).HasMaxLength(50);
                entity.Property(e => e.BuildingTypeId);
                entity.Property(e => e.ParishName).HasMaxLength(50);
                entity.Property(e => e.ParishNumber).HasMaxLength(50);
                entity.Property(e => e.RoomName).HasMaxLength(255);
                entity.Property(e => e.ElectoralDistrictNumber).HasMaxLength(50);
                entity.Property(e => e.ConstituencyName).HasMaxLength(50);
                entity.Property(e => e.ConstituencyNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<PersonView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.PersonId);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.PersonView);

                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.ParishName).HasMaxLength(103);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");
                entity.Property(e => e.CourseOccassionAddress).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionDate).HasColumnType("datetime");
                entity.Property(e => e.CourseOccassionCourseInfo).HasMaxLength(250);
                entity.Property(e => e.CourseOccassionCourseDescription).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionRoom).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionHall).HasMaxLength(50);
                entity.Property(e => e.CourseOccassionEndTime).HasPrecision(0);
                entity.Property(e => e.CourseOccassionStartTime).HasPrecision(0);
                entity.Property(e => e.Length);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo)
                    .HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonGenderId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonTag).HasMaxLength(50);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.PersonTypeId);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.Temp)
                    .HasMaxLength(1);
                entity.Property(e => e.AssignmentFunction).HasMaxLength(50);
                entity.Property(e => e.AssignmentPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ConstituencyName).HasMaxLength(101);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");

            });

            modelBuilder.Entity<ElectionView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.ElectionId);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.ElectionView);

                entity.Property(e => e.BuildingName).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingDistrict).HasMaxLength(50);
                entity.Property(e => e.BuildingDistrictId);
                entity.Property(e => e.BuildingStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingType).HasMaxLength(50);
                entity.Property(e => e.BuildingTypeId);
                entity.Property(e => e.ParishName).HasMaxLength(50);
                entity.Property(e => e.ParishNumber).HasMaxLength(50);
                entity.Property(e => e.GruppNamn).HasMaxLength(100);
                entity.Property(e => e.RoomName).HasMaxLength(255);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonTyp).HasMaxLength(50);
                entity.Property(e => e.ElectionDate).HasColumnType("datetime");
                entity.Property(e => e.ElectionName).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictNumber).HasMaxLength(50);
                entity.Property(e => e.ConstituencyName).HasMaxLength(50);
                entity.Property(e => e.ConstituencyNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<ElectoralDistrictView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.ElectionId);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.ElectoralDistrictView);

                entity.Property(e => e.BuildingName).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingVisitingPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingEmail).HasMaxLength(255);
                entity.Property(e => e.BuildingWebpage).HasMaxLength(255);
                entity.Property(e => e.BuildingInformerNeeded)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingCanExpand)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingDeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalCode).HasMaxLength(255);
                entity.Property(e => e.BuildingDeliveryPostalPlace).HasMaxLength(255);
                entity.Property(e => e.BuildingDistrict).HasMaxLength(50);
                entity.Property(e => e.BuildingDistrictId);
                entity.Property(e => e.BuildingStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.BuildingPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.BuildingType).HasMaxLength(50);
                entity.Property(e => e.BuildingTypeId);
                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.ParishName).HasMaxLength(50);
                entity.Property(e => e.ParishNumber).HasMaxLength(50);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.RoomName).HasMaxLength(255);
                entity.Property(e => e.PersonAddress).HasMaxLength(255);
                entity.Property(e => e.PersonCar)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.PersonCo)
                    .HasMaxLength(255);
                entity.Property(e => e.PersonLastName).HasMaxLength(255);
                entity.Property(e => e.PersonEmail).HasMaxLength(255);
                entity.Property(e => e.PersonFirstName).HasMaxLength(255);
                entity.Property(e => e.PersonGender)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.PersonMobilePhone).HasMaxLength(255);
                entity.Property(e => e.PersonSocialSecurityNumber).HasMaxLength(50);
                entity.Property(e => e.PersonPostalCode).HasMaxLength(255);
                entity.Property(e => e.PersonPostalPlace).HasMaxLength(255);
                entity.Property(e => e.PersonStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false);
                entity.Property(e => e.PersonStatusId)
                    .HasMaxLength(36)
                    .IsUnicode(false);
                entity.Property(e => e.PersonPhoneNumber).HasMaxLength(255);
                entity.Property(e => e.PersonType).HasMaxLength(50);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.AssignmentFunction).HasMaxLength(50);
                entity.Property(e => e.AssignmentPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ElectoralDistrictNumber).HasMaxLength(50);
                entity.Property(e => e.ConstituencyName).HasMaxLength(50);
                entity.Property(e => e.ConstituencyNumber).HasMaxLength(50);
                entity.Property(e => e.ApplicationOfInterestCreated).HasColumnType("datetime");

            });

            modelBuilder.Entity<AssignmentView>(entity =>
            {
                if (isInMemory)
                    entity.HasKey(e => e.AssignementFunction);
                else
                    entity.HasNoKey();

                entity.ToView(ViewsConstants.AssignmentView);

                entity.Property(e => e.FeeType)
                    .HasMaxLength(6)
                    .IsUnicode(false);
                entity.Property(e => e.ParishName).HasMaxLength(103);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.Accepted)
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.AssignementFunction).HasMaxLength(50);
                entity.Property(e => e.AssignementPublicFunction).HasMaxLength(50);
                entity.Property(e => e.ElectoralDistrictName).HasMaxLength(101);
                entity.Property(e => e.ConstituencyName).HasMaxLength(101);
            });
        }
    }
}
