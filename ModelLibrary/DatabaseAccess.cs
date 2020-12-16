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

        public static int SaveMember(MemberModel member)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("insert into member(memberName, memberTel, memberAddr) values (@MemberName, @MemberTel, @MemberAddr); SELECT last_insert_rowid()", member);
                return output;
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

        public static int SaveDestination(DestinationModel destination)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO destination(desName, province) VALUES(@DesName, @Province); SELECT last_insert_rowid()", destination);
                return output;
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

        public static int SaveJourneyDestination(JourneyDestinationModel destination)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO journey_destination(journeyId, desId) VALUES(@JourneyId, @DesId)", destination);
                return output;
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

        public static int SaveJourney(JourneyModel journey)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.ExecuteScalar<int>("INSERT INTO journey(journeyName, journeyDescription, startDate, endDate, coverImage) VALUES(@JourneyName, @JourneyDescription, @StartDate, @EndDate, @CoverImage); SELECT last_insert_rowid()", journey);
                return output;
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

        public static int SaveJourneyMember(int journeyId, int memberId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO journey_member(journeyId, memberId) VALUES(@JourneyId, @MemberId); SELECT last_insert_rowid()", new { JourneyId = journeyId, MemberId = memberId});
                return output;
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

        public static int SavePayment(PaymentModel payment)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO payment(memberId, journeyId, paymentContent, paymentValue) VALUES(@MemberId, @JourneyId, @PaymentContent, @PaymentValue)", payment);
                return output;
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

        public static int SaveRoute(RouteModel route)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO route(journeyId, routeContent) VALUES (@JourneyId, @RouteContent); SELECT last_insert_rowid()", route);
                return output;
            }
        }

        //Get payment sum per member
        public static List<PaymentPerMemberModel> LoadPaymentPerMember(int journeyId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<PaymentPerMemberModel>($"SELECT payment.memberId, member.memberName, sum(payment.paymentValue) AS paymentValue FROM member JOIN payment ON member.memberId = payment.memberId WHERE payment.journeyId = {journeyId} GROUP BY payment.memberId, member.memberName", new DynamicParameters());
                return output.ToList();
            }
        }

        private static string LoadConnectionString(string id = "default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
