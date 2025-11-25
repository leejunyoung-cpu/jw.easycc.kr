using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonLibrary.DBModel
{
    public class ReqApiIpWhiteList
    {
        public int       SeqNo    { get; set; }                    // 
        public string    ServiceCode    { get; set; }              // 
        public string    Company    { get; set; }                  // 
        public string    IpAddress    { get; set; }                // 
        public string    IsActive    { get; set; }                 // 
        public string    ServiceUrl    { get; set; }               // 
        public string    IpNote    { get; set; }                   //            
        public int      PageSize        { get; set; }
        public int      PageNo          { get; set; }
    }

    public class ResApiIpWhiteList
    {
        public List<ApiIpWhiteViewModel> List { get; set; }
        public int RecordCnt { get; set; }
    }

    public class ApiIpWhiteViewModel
    {
        public int       SeqNo          { get; set; }
        public string    ServiceCode    { get; set; }
        public string    Company        { get; set; }
        public string    IpAddress      { get; set; }
        public string    IsActive       { get; set; }
        public string    ServiceUrl     { get; set; }
        public string    IpNote         { get; set; }
    }

    public class ReqApiHuaweiList
    {
        public string DateFrom  { get; set; }
        public string DateTo    { get; set; }
        public int    PageSize { get; set; }
        public int    PageNo { get; set; }
    }

    public class ResApiHuaweiList
    {
        public List<ApiHuaweiViewModel> List { get; set; }
        public int RecordCnt { get; set; }
    }

    public class ApiHuaweiViewModel
    { 
        public long    HuaweiHeaderId    { get; set; }           // 
        public long    InboxId    { get; set; }                  // 
        public long    declarationHeaderId    { get; set; }      // 세관신고서 ID
        public string    impExpFlag    { get; set; }               // 수출입통관서표지
        public string    preDeclarationNumber    { get; set; }     // 통관초안번호
        public string    declarationCompanyCode    { get; set; }   // 신고회사코드
        public string    declarationCompanyName    { get; set; }   // 통관회사명
        public string    countryCode    { get; set; }              // 국가 부호
        public string    shipper    { get; set; }                  // 수취인 번호
        public string    shipperName    { get; set; }              // 발송인명
        public string    consignee    { get; set; }                // 수입자
        public string    consigneeName    { get; set; }            // 수입자명
        public string    tradeMode    { get; set; }                // 무역방식
        public string    tradeTerm    { get; set; }                // 무역용어
        public string    transportMode    { get; set; }            // 운송방식
        public string    transportNo    { get; set; }              // 운송수단 번호
        public string    transportRefNo    { get; set; }           // 수송편
        public string    departureCountryCode    { get; set; }     // 선적국 2자
        public string    departureCountryShortName    { get; set; }// 출발국 중국어 약칭
        public string    customsBroker    { get; set; }            // 통관 번호
        public string    customsBrokerName    { get; set; }        // 통관대행업체명
        public string    seller    { get; set; }                   // 판매자 번호
        public string    sellerName    { get; set; }               // 매도인명
        public string    buyer    { get; set; }                    // 구매자 코드
        public string    buyerName    { get; set; }                // 구매자 명
        public string    invoiceCurrency    { get; set; }          // 송장통화
        public double    invoiceAmount    { get; set; }            // 송장 금액
        public double    exchangeRate    { get; set; }             // 환율
        public string    containerSpecification    { get; set; }   // 컨테이너 규격
        public string    hawb    { get; set; }                     // 운송장 번호
        public string    mawb    { get; set; }                     // 마스터 송장 번호
        public string    departurePort    { get; set; }            // 선적항
        public string    destinationPort    { get; set; }          // 도착항
        public string    currency    { get; set; }                 // 통관통화
        public double    totalAmount    { get; set; }              // 통관신고금액
        public double    freight    { get; set; }                  // 운임(운송료)
        public double    insurance    { get; set; }                // 보험료
        public double    otherFee    { get; set; }                 // 잡비
        public int    totalPackages    { get; set; }            // 총 건수
        public double    totalNetWeight    { get; set; }           // 총순중량
        public double    totalGrossWeight    { get; set; }         // 총중량
        public double    totalVolume    { get; set; }              // 총체적
        public double    fobAmount    { get; set; }                // 통관신고서 FOB금액
        public double    cifAmount    { get; set; }                // 통관신고서 CIF 금액
        public string    invnumber    { get; set; }                // 인보이스번호
        public string    plNo    { get; set; }                     // 상자 번호
        public string    contractNumber    { get; set; }           // 계약번호
        public string    shipmentId    { get; set; }               // 운송 방안 번호
        public string    Status    { get; set; }                   // 
        public string    ErrorMessage    { get; set; }             // 
        public string    RegDate    { get; set; }                  // 
        public string    UpdDate    { get; set; }                  // 

        public long HuaweiLineId { get; set; }             // 
        public long declarationLineId { get; set; }        // 통관서류명세서
        public int lineNo { get; set; }                   // 행호
        public string itemCode { get; set; }                 // 재료코드
        public string itemDescription { get; set; }          // 재료설명
        public string hsCode { get; set; }                   // 세번부호
        public string goodsName { get; set; }                // 세관 상품 중국어 명칭
        public string goodsNameEn { get; set; }              // 세관 상품 영문명칭
        public string brand { get; set; }                    // 브랜드
        public string specification { get; set; }            // 모델
        public string originCountryCode { get; set; }        // 원산지 국가 2자 코드
        public string originCountryName { get; set; }        // 원산국명
        public string unit { get; set; }                     // 단위
        public double quantity { get; set; }                 // 수량
        public double price { get; set; }                    // 단가
        public double amount { get; set; }                   // 금액
        public double netWeight { get; set; }                // 순중량
        public double totalGrossWeight2 { get; set; }         // 재료 총중량
        public double invoiceFobAmount { get; set; }         // fob 금액
        public string licnumber { get; set; }                // 영업 허가증 번호
    }
}