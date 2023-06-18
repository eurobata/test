using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TemplateManagementSystem.Models
{
    public class DailyTimeSheetViewModel
    {
        public DailyTimeSheetViewModel()
        {
            lstDetails = new List<DailyTimeSheetDetailViewModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required.")]

        public string Subject { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]

        public string CC { get; set; }
        public string EmailBody { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
        public string type { get; set; }

        public List<DailyTimeSheetDetailViewModel> lstDetails { get; set; }

    }

    public class TemplateSearchModel
    {
        public string TemplateName { get; set; }
        public string TemplateCategory { get; set; }
        public string TemplateClass { get; set; }
    }


    public class PressureWashingViewModel
    {
        public PressureWashingViewModel()
        {
            templetItems = new List<WashingTempletItems>();
        }
        [Required]
        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required.")]

        public string Subject { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]

        public string CC { get; set; }
        public string EmailBody { get; set; }

        public DateTime date { get; set; }
        public string InvoiceId { get; set; }
        public string For { get; set; }

        public string Address { get; set; }
        public string type { get; set; }
        [NotMapped]
        public string userProfile { get; set; }
        public string Notes { get; set; }
        public string PreparedBy { get; set; }
        public decimal subtotall { get; set; }

        public decimal tax { get; set; }
        public decimal totall { get; set; }
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime createdAt { get; set; }
        [NotMapped]
        public string datetimestring { get; set; }
        public string Filename { get; set; }
        [NotMapped]
        public string logo { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<WashingTempletItems> templetItems { get; set; }

    }
    public class WashingTempletItems
    {
        public int id { get; set; }
        public string item { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal ammount { get; set; }

    }

    public class BussinessQuotesViewModel
    {
        public BussinessQuotesViewModel()
        {
            items = new List<bussinessQuotesItems>();
        }
        [Required]
        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required.")]

        public string Subject { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]

        public string CC { get; set; }
        public string EmailBody { get; set; }
        [NotMapped]
        public string ExpiryDateStr { get; set; }
        [NotMapped]
        public string issueDatestr { get; set; }

        public DateTime expirydate { get; set; }
        public DateTime issuedate { get; set; }

        public string quoteTxt { get; set; }

        [NotMapped]
        public string ECDStr { get; set; }


        [NotMapped]
        public string ECoDStr { get; set; }

        public DateTime ecd { get; set; }

        public DateTime ecod { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }

        public string type { get; set; }


        public decimal subtotall { get; set; }
        public decimal discount { get; set; }
        public decimal totallTaxAmount { get; set; }
        public decimal shipingHandling { get; set; }

        public decimal tax { get; set; }
        public decimal totall { get; set; }
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime createdAt { get; set; }
        
        public string Filename { get; set; }
        [NotMapped]
        public string logo { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<bussinessQuotesItems> items { get; set; }

    } 
   
    public class bussinessQuotesItems
    {

        public int id { get; set; }
        public string item { get; set; }
        public decimal ammount { get; set; }

    }

    public class CustomerTempleteModel
    {
           public CustomerTempleteModel()
        {
            item = new List<customerItems>();
        }
        [Required]
        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required.")]

        public string Subject { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]

        public string CC { get; set; }
        public string UserName { get; set; }
        public string userAdress { get; set; }
        public string userCountry { get; set; }
        public string useroffice { get; set; }
        public string userPostal { get; set; }
        public string userPhone { get; set; }
        public string useremail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

        public string userProfile { get; set; }
        [NotMapped]
        public string type { get; set; }

        public decimal dscount { get; set; }
        public decimal totall { get; set; }
        public string EmailBody { get; set; }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime createdAt { get; set; }

        public string Filename { get; set; }
        public decimal subtotall { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public List<customerItems> item { get; set; }

    }
    public class EmailSetup
    {
        public int id { get; set; }
        public string templete { get; set; }
        [Required]
        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject is required.")]

        public string Subject { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]

        public string CC { get; set; }
        public string EmailBody { get; set; }
    
    }
    public class customerItems
    {
        public int id { get; set; }
        public string service { get; set; }
        public int  quantity { get; set; }
        public decimal  price { get; set; }
        public decimal  totall { get; set; }

        public string decription { get; set; }
    }
    public class DailyTimeSheetDetailViewModel
    {
        public int Id { get; set; }
        public string JobDescription { get; set; }
        public Nullable<System.TimeSpan> TimeStarted { get; set; }
        public Nullable<System.TimeSpan> TimeStopped { get; set; }
        public string Initials { get; set; }
    }

    public class CallOutReportViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Job NO. is required.")]
        public string JobNo { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string JobAddress { get; set; }
        [Required]
        public string ContactTelephone { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string DateStr { get; set; }
        [Required]
        public string EngineerName { get; set; }
        [Required]
        public string DetailOfWork { get; set; }
        [Required]
        public Nullable<System.TimeSpan> ArrivalTime { get; set; }
        [Required]
        public Nullable<System.TimeSpan> DepartTime { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string EmailBody { get; set; }
        public string type { get; set; }
    }


    

    public class Template5ViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }
        public string DateStr { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string CoordinatorName { get; set; }
        [Required]
        public string Activity { get; set; }
        [Required]
        public string NoOfLeaders { get; set; }
        public string RiskAnalysis { get; set; }
        public string CasualFactorPeople { get; set; }
        public string CasualFactorEquipment { get; set; }
        public string CasualFactorEnvironment { get; set; }
        public string NormalOperationPeople { get; set; }
        public string NormalOperationEquipment { get; set; }
        public string NormalOperationEnvironment { get; set; }
        public string EmergencyPeople { get; set; }
        public string EmergencyEquipment { get; set; }
        public string EmergencyEnvironment { get; set; }
        public string SkillsRequiredByLeaders { get; set; }
        public string FormCompletedBy { get; set; }
        public Nullable<System.DateTime> FormCompletionDate { get; set; }
        public string FormCompletionDateStr { get; set; }
        public Nullable<bool> ActivityStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string ApprovedDateStr { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string EmailBody { get; set; }
        public string type { get; set; }
    
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        //public List<string> Templates { get; set; }
    }

    public class TemplateCategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }
        public List<int?> TemplateIds { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TemplateSelected { get; set; }
        public List<SelectListItem> Templates { get; set; }
        //public List<string> Templates { get; set; }
    }
    public class RiskManagement
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string issueDatestr { get; set; }

        public string siteLocation { get; set; }
        public DateTime Date { get; set; }

        public string supervisorLeading { get; set; }
        [DataType(DataType.MultilineText)]

        public string workActivityDetail { get; set; }

        public string Excavation { get; set; }
        public string workatHeight { get; set; }
        public string spaceEntry { get; set; }
        public string other { get; set; }
        public string hotWork { get; set; }
        public string wOverHead { get; set; }
        public string tool1 { get; set; }
        public string tool2 { get; set; }
        public string tool3 { get; set; }
        public string tool4 { get; set; }
        public string tool5 { get; set; }
        public string tool6 { get; set; }
        public string tool7 { get; set; }
        public string tool8 { get; set; }
        public string tool9 { get; set; }
        public string AdditionComment { get; set; }
        public string sds { get; set; }
        public string slipTrip { get; set; }
        public string plantrafic { get; set; }
        public string hazardOsb { get; set; }

        [NotMapped]
        public string logo { get; set; }
        [NotMapped]
        public string addressStr { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }

        public DateTime createdAt { get; set; } = DateTime.Now;


    }


    public class ACQuote
    {
        [NotMapped]
        public string type { get; set; }


        public int Id { get; set; }

        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string issueDatestr { get; set; }

        public string uniqueid { get; set; }
        public DateTime Date { get; set; }

        public string address { get; set; }
        public string name { get; set; }
        public string b2b { get; set; }
        public string capping { get; set; }

        
        public string locationAc { get; set; }

        public string replacement { get; set; }
        public string ExtraPipe { get; set; }
        public string sizeOfAc { get; set; }
        public string mounting { get; set; }
        public string wsm { get; set; }
        public string nefr { get; set; }
        public string drain { get; set; }
        public string rcdRcbo { get; set; }
        public string addtionalinfo { get; set; }
        public string image1 { get; set; }
        public string image1Des { get; set; }
        public string image2 { get; set; }
        public string image2Des { get; set; }
        public string image3 { get; set; }
        public string image3Des { get; set; }
        public string image4 { get; set; }
        public string image4Des { get; set; }
        public string image5 { get; set; }
        public string image5Des { get; set; }
        public string image6 { get; set; }
        public string image6Des { get; set; }

       

        [NotMapped]
        public string logo { get; set; }
        [NotMapped]
        public string addressStr { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;


    }

    public class DomesticSmokeAlarm
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string issueDatestr { get; set; }

        public string uniqueid { get; set; }
        public DateTime Date { get; set; }

        public string Nameaddress { get; set; }


        public string LisenceNo { get; set; }

        public string InspectionAddress { get; set; }
        public string buildingClass { get; set; }
        [NotMapped]
        public string InspectionDateStr { get; set; }
        public DateTime InspectionDate { get; set; }

        public string InspectedBy { get; set; }

        public bool check1 { get; set; }
        public bool check2 { get; set; }

        public string VoltageSmoke1 { get; set; }
        public string VoltageSmoke2 { get; set; }
        public string VoltageSmoke3 { get; set; }
        public string VoltageSmoke4 { get; set; }
        public string VoltageSmoke5 { get; set; }
        public string LocationSmoke1 { get; set; }
        public string LocationSmoke2 { get; set; }
        public string LocationSmoke3 { get; set; }
        public string LocationSmoke4 { get; set; }
        public string LocationSmoke5 { get; set; }
        public string RoomSmoke1 { get; set; }
        public string RoomSmoke2 { get; set; }
        public string RoomSmoke3 { get; set; }
        public string RoomSmoke4 { get; set; }
        public string RoomSmoke5 { get; set; }
        public string ExpireSmoke1 { get; set; }
        public string ExpireSmoke2 { get; set; }
        public string ExpireSmoke3 { get; set; }
        public string ExpireSmoke4 { get; set; }
        public string ExpireSmoke5 { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }


        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;



    }
    public class DomesticSmokeAlarmII
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string issueDatestr { get; set; }

        public string uniqueid { get; set; }
        public DateTime Date { get; set; }

        public string Nameaddress { get; set; }


        public string LisenceNo { get; set; }

        public string InspectionAddress { get; set; }
        public string buildingClass { get; set; }
        [NotMapped]
        public string InspectionDateStr { get; set; }
        public DateTime InspectionDate { get; set; }

        public string InspectedBy { get; set; }

        public bool check1 { get; set; }
        public bool check2 { get; set; }

        public string VoltageSmoke1 { get; set; }
        public string VoltageSmoke2 { get; set; }
        public string VoltageSmoke3 { get; set; }
        public string VoltageSmoke4 { get; set; }
        public string VoltageSmoke5 { get; set; }
        public string LocationSmoke1 { get; set; }
        public string LocationSmoke2 { get; set; }
        public string LocationSmoke3 { get; set; }
        public string LocationSmoke4 { get; set; }
        public string LocationSmoke5 { get; set; }
        public string RoomSmoke1 { get; set; }
        public string RoomSmoke2 { get; set; }
        public string RoomSmoke3 { get; set; }
        public string RoomSmoke4 { get; set; }
        public string RoomSmoke5 { get; set; }
        public string ExpireSmoke1 { get; set; }
        public string ExpireSmoke2 { get; set; }
        public string ExpireSmoke3 { get; set; }
        public string ExpireSmoke4 { get; set; }
        public string ExpireSmoke5 { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }


        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;



    }

    public class covid19
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string datestr2 { get; set; }

        [NotMapped]
        public string locationLin1 { get; set; }
        [NotMapped]
        public string city { get; set; }
        [NotMapped]
        public string state { get; set; }
        [NotMapped]
        public string postcode { get; set; }
        [NotMapped]
        public string phone { get; set; }

        [NotMapped]
        public string email { get; set; }

        public string jobAddress { get; set; }
        public string jobnumber { get; set; }
        public string firstName { get; set; }
        public string lastname { get; set; }
        public string sign { get; set; }

        public DateTime Date { get; set; }
        public DateTime Date2 { get; set; }

       
        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }



    }
    public class JobCompletionII
    {
        [NotMapped]
        public string type { get; set; }


        public int Id { get; set; }

        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string datestr2 { get; set; }

        [NotMapped]
        public string locationLin1 { get; set; }
        [NotMapped]
        public string locationLin2 { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public string website { get; set; }


        [NotMapped]
        public string city { get; set; }
        [NotMapped]
        public string state { get; set; }
        [NotMapped]
        public string postcode { get; set; }
        [NotMapped]
        public string phone { get; set; }

        [NotMapped]
        public string email { get; set; }

        public string Job { get; set; }
        public string clientName { get; set; }
        public string siteAddress { get; set; }
        public string callBack { get; set; }
        public string beforPhoto { get; set; }
        public string afterPhoto { get; set; }
        public string issueReported { get; set; }
        public string workCarried { get; set; }
        public string additionalNotes { get; set; }
        public string MaterialUsed { get; set; }
        public string cusomername { get; set; }
        public string sign { get; set; }
        public DateTime date { get; set; }

        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }


    }
    public class JobCompletion
    {
        [NotMapped]
        public string type { get; set; }


        public int Id { get; set; }

        [NotMapped]
        public string datestr { get; set; }
        [NotMapped]
        public string datestr2 { get; set; }

        [NotMapped]
        public string locationLin1 { get; set; }
        [NotMapped]
        public string locationLin2 { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public string website { get; set; }


        [NotMapped]
        public string city { get; set; }
        [NotMapped]
        public string state { get; set; }
        [NotMapped]
        public string postcode { get; set; }
        [NotMapped]
        public string phone { get; set; }

        [NotMapped]
        public string email { get; set; }

        public string Job { get; set; }
        public string clientName { get; set; }
        public string siteAddress { get; set; }
        public string callBack { get; set; }
        public string beforPhoto { get; set; }
        public string afterPhoto { get; set; }
        public string issueReported { get; set; }
        public string workCarried { get; set; }
        public string additionalNotes { get; set; }
        public string MaterialUsed { get; set; }
        public string cusomername { get; set; }
        public string sign { get; set; }
        public DateTime date { get; set; }


        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }

    }
    public class CleaningCheckList
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }
       

        public string clientName { get; set; }
        public string address { get; set; }
        public string jobrefer { get; set; }
        public DateTime date { get; set; }
        public string afterPhoto { get; set; }
        public bool check1 { get; set; }
        public bool check2 { get; set; }
        public bool check3 { get; set; }
        public bool check4 { get; set; }
        public bool check5 { get; set; }
        public bool check6 { get; set; }
        public bool check7 { get; set; }
        public bool check8 { get; set; }
        public bool check9 { get; set; }
        public bool check10 { get; set; }
        public bool check11 { get; set; }
        public bool check12 { get; set; }
        public bool check13 { get; set; }
        public string comments { get; set; }

        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }


    }
    public class ServiceBoiler
    {
        public int Id { get; set; }
        [NotMapped]
        public string type { get; set; }


        [NotMapped]
        public string datestr { get; set; }


        public string BussinessDetail { get; set; }
        public string Siteaddress { get; set; }
        public string CustomerDetails { get; set; }
        public string orType { get; set; }
        public string make { get; set; }
        public string aproxAge { get; set; }

        public string apploc { get; set; }
        public string model { get; set; }

        public string PropertyRented { get; set; }
        public string MagneticFiler { get; set; }
        public string conditionOfElectric { get; set; }

        public string expensionVessel { get; set; }
        public string controlsystem { get; set; }
        public string condensateTrap { get; set; }
        public string ignitionProcess { get; set; }
        public string burner { get; set; }
        public string flampicture { get; set; }
        public string sparkElectrodes { get; set; }
        public string rectification { get; set; }
        public string exchanger { get; set; }
        public string waterQuality { get; set; }
        public string location { get; set; }
        public string ApStability { get; set; }
        public string GassWaterLeak { get; set; }
        public string fanCondition { get; set; }
        public string pumpCondition { get; set; }
        public string automaticAirVent { get; set; }
        public string pipConnection { get; set; }
        public string sealCondition { get; set; }
        public string tightNess { get; set; }
        public string gasTightness { get; set; }
        public string clearences { get; set; }
        public string operatingPressuer { get; set; }
        public string safetyDevice { get; set; }
        public string ventilationRequiremnts { get; set; }
        public string flueflow { get; set; }
        public string combustion { get; set; }
        public string intake { get; set; }
        public string fluetermination { get; set; }
        public string MincarbonMonoOxide { get; set; }
        public string Minoxygenlevel { get; set; }
        public string MincarbDioxide { get; set; }
        public string MinRatioCoCo2 { get; set; }
        public string MaxcarbonMonoOxide { get; set; }
        public string Maxoxygenlevel { get; set; }
        public string MaxcarbDioxide { get; set; }
        public string MaxRatioCoCo2 { get; set; }
        public string IntcarbonMonoOxide { get; set; }
        public string Intoxygenlevel { get; set; }
        public string IntcarbDioxide { get; set; }
        public string IntRatioCoCo2 { get; set; }

        public int gasrate { get; set; }
        public int workingPressure { get; set; }
        public int boilerPressuer { get; set; }
        public int burnerPressure { get; set; }
        public string finding1 { get; set; }
        public string finding2 { get; set; }
        public string finding3 { get; set; }
        public string comment { get; set; }
        public string engName { get; set; }
        public string LicsenceNo { get; set; }
        public string recievedBy { get; set; }
        public string position { get; set; }
        public DateTime date { get; set; }
        public DateTime Duedate { get; set; }
        [NotMapped]
        public string duDatestr { get; set; }

        public string certificateNo { get; set; }

        public string sign { get; set; }


        [NotMapped]
        public string logo { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string FileName { get; set; }


    }

}