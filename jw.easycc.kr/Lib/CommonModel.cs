using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CommonLibrary.CommonModel
{
    public class ResponseMap
    {
        //공통 멤버 변수
        public int RetCode { get; set; }
        public string ErrMsg { get; set; }
        public string DevErrMsg { get; set; }

        public string strResponse = string.Empty;

        //추가 데이타를 담기 위한 Dictionary
        readonly Dictionary<String, Object> dic = null;
        readonly JavaScriptSerializer objJsonSerializer = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public ResponseMap()
        {
            //초기화
            dic = new Dictionary<string, object>();
            objJsonSerializer = new JavaScriptSerializer();
        }

        /// <summary>
        /// Dictionary 데이터 저장
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">value</param>
        public void Add(String key, Object obj)
        {
            dic.Add(key, obj);
        }

        /// <summary>
        /// Dictionary 데이터 삭제
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(String key)
        {
            return dic.Remove(key);
        }

        /// <summary>
        /// Dictionary 초기화
        /// </summary>
        public void Clear()
        {
            dic.Clear();
        }

        /// <summary>
        /// Dictionary 데이터 조회
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public Object Get(String key)
        {
            return dic[key];
        }


        /// <summary>
        /// Dictionary 에 저장된 데이터를 Json String 으로 변환한다 
        /// - 에러코드와 에러메시지도 함께 담는다 (this.intRetval, this.strErrMsg)
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            string lo_strSerialize = string.Empty;
            string lo_strContentTypeJson = "application/json";

            //strResponse가 있으면 리턴
            if (!string.IsNullOrEmpty(strResponse))
            {
                return strResponse;
            }

            // 설정된 contexttype이 JSON 출력이 아닌 경우 null 반환 (파일다운로드 처리로직)
            if (!HttpContext.Current.Response.ContentType.Equals(lo_strContentTypeJson))
            {
                return null;
            }

            //익셉션 발생시에는 공통 메시지
            dic.Add("ErrMsg", RetCode < 0 ? "Exception Error" : ErrMsg);
            dic.Add("RetCode", RetCode);

            lo_strSerialize = "[" + objJsonSerializer.Serialize(dic) + "]";

            return lo_strSerialize;
        }
    }

    ///-------------------------------------
    /// <summary>
    /// 에러 데이터
    /// </summary>
    ///-------------------------------------
    public class ErrResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public int DevErrorCode { get; set; }
        public string DevErrorMsg { get; set; }
        public string ReturnUrl { get; set; }

        public ErrResult()
        {
            ErrorCode = 0;
            DevErrorCode = 0;
        }

        public ErrResult(int intRetVal, string strErrMsg)
        {
            ErrorCode = intRetVal;
            ErrorMsg = strErrMsg;
        }

        public ErrResult(int intRetVal, string strErrMsg, int intDetailRetVal, string strDetailErrMsg)
        {
            ErrorCode = intRetVal;
            ErrorMsg = strErrMsg;
            DevErrorCode = intDetailRetVal;
            DevErrorMsg = strDetailErrMsg;
        }
    }

    /// <summary>
    /// 공통 응답 처리
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// 성공 응답
        /// </summary>
        public T data { get; set; }

        public ErrResult result { get; set; }

        public ServiceResult()
        {
            result = new ErrResult();
        }

        public ServiceResult(T t)
        {
            data = t;
            result = new ErrResult();
        }

        public ServiceResult(int code_, string message_ = null, int detailCode_ = 0, string detail_ = null)
        {
            SetResult(code_, message_, detailCode_, detail_);
        }

        internal void SetResult(int code_, string message_ = null, int detailCode_ = 0, string detail_ = null)
        {
            result = new ErrResult(code_, message_, detailCode_, detail_);
        }
    }

    public class ErrorResponse
    {
        public ErrResult result { get; set; }
        public ErrorResponse()
        {
            result = new ErrResult();
        }
    }
}