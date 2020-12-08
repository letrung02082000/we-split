using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class DatabaseAccess
    {
        //CRUD Member
        public static List<MemberModel> LoadMember()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<MemberModel>("select * from member", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveMember(MemberModel member)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute("insert into member(memberName, memberTel, memberAddr) values (@MemberName, @MemberTel, @MemberAddr)", member);
            }
        }

        public static void UpdateMember(MemberModel member)
        {
            using(IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute($"UPDATE member SET memberName = {member.MemberName}, memberTel = {member.MemberTel}, memberAddr = {member.MemberAddr} WHERE memberId = {member.MemberId}");
            }
        }

        //CRUD Destination
        public static List<DestinationModel> LoadDestination()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<DestinationModel>("select * from destination", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveDestination(DestinationModel destination)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Execute("INSERT INTO destination(desName, province) VALUES(@DesName, @Province)", destination);
            }
        }

        public static void UpdateDestination(DestinationModel destination)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Execute("UPDATE destination SET desName = @DesName, province = @Province WHERE desId = @DesId", destination);
            }
        }

        public static List<DestinationModel> LoadJourneyDestination(int journeyId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<DestinationModel>($"select destination.desId, destination.desName, destination.province from destination join journey_destination on destination.desId = journey_destination.desId where journey_destination.journeyId = {journeyId}", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveJourneyDestination(JourneyDestinationModel destination)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Execute("INSERT INTO journey_destination(journeyId, desId) VALUES(@JourneyId, @DesId)", destination);
            }
        }

        //CRUD Journey
        public static List<JourneyModel> LoadJourney()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<JourneyModel>("select * from journey", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveJourney(JourneyModel journey)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Execute("INSERT INTO journey(journeyName, journeyDescription, startDate, endDate, coverImage) VALUES(@JourneyName, @JourneyDescription, @StartDate, @EndDate, @CoverImage)", journey);
            }
        }

        public static void UpdateJourney(JourneyModel journey)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Execute("UPDATE journey SET journeyName = @JourneyName, journeyDescription = @JourneyDescription, startDate = @StartDate, endDate = @EndDate, coverImage = @CoverImage WHERE journeyId = @JourneyId", journey);
            }
        }

        //CRUD JourneyMember
        public static List<MemberModel> LoadJourneyMember(int journeyId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<MemberModel>($"select member.memberId, member.memberName, member.memberTel, member.memberAddr from journey_member join member on journey_member.memberId = member.memberId where journey_member.journeyId={journeyId}", new DynamicParameters());
                return output.ToList();
            }
        }

        //CRUD Payment
        public static List<PaymentModel> LoadPayment(int journeyId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<PaymentModel>($"select payment.journeyId, payment.memberId, payment.paymentContent, payment.paymentValue, member.memberName from payment join member on payment.memberId = member.memberId where payment.journeyId = {journeyId}", new DynamicParameters());
                return output.ToList();
            }
        }

        //CRUD Route
        public static List<RouteModel> LoadRoute(int journeyId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<RouteModel>($"select * from route where route.journeyId = {journeyId} order by route.routeId asc", new DynamicParameters());
                return output.ToList();
            }
        }

        private static string LoadConnectionString(string id = "default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
