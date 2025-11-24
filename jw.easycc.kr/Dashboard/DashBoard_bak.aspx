<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard_bak.aspx.cs" Inherits="easycc.co.kr.DashBoard_bak" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <!--
        <link rel="stylesheet" href="/css/app-light.css" id="lightTheme">
        <link rel="stylesheet" href="/css/app-dark.css" id="darkTheme" disabled>
        <div class="container-fluid">
          <div class="row justify-content-center">
            <div class="col-12">
              <div class="row">
                <div class="col-md-6 col-xl-3 mb-4">
                  <div class="card shadow bg-primary text-white border-0">
                    <div class="card-body">
                      <div class="row align-items-center">
                        <div class="col-3 text-center">
                          <span class="circle circle-sm bg-primary-light">
                            <i class="fe fe-16 fe-shopping-bag text-white mb-0"></i>
                          </span>
                        </div>
                        <div class="col pr-0">
                          <p class="small text-muted mb-0">Monthly Sales</p>
                          <span class="h3 mb-0 text-white">$1250</span>
                          <span class="small text-muted">+5.5%</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="col-md-6 col-xl-3 mb-4">
                  <div class="card shadow border-0">
                    <div class="card-body">
                      <div class="row align-items-center">
                        <div class="col-3 text-center">
                          <span class="circle circle-sm bg-primary">
                            <i class="fe fe-16 fe-shopping-cart text-white mb-0"></i>
                          </span>
                        </div>
                        <div class="col pr-0">
                          <p class="small text-muted mb-0">Orders</p>
                          <span class="h3 mb-0">1,869</span>
                          <span class="small text-success">+16.5%</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="col-md-6 col-xl-3 mb-4">
                  <div class="card shadow border-0">
                    <div class="card-body">
                      <div class="row align-items-center">
                        <div class="col-3 text-center">
                          <span class="circle circle-sm bg-primary">
                            <i class="fe fe-16 fe-filter text-white mb-0"></i>
                          </span>
                        </div>
                        <div class="col">
                          <p class="small text-muted mb-0">Conversion</p>
                          <div class="row align-items-center no-gutters">
                            <div class="col-auto">
                              <span class="h3 mr-2 mb-0"> 86.6% </span>
                            </div>
                            <div class="col-md-12 col-lg">
                              <div class="progress progress-sm mt-2" style="height:3px">
                                <div class="progress-bar bg-success" role="progressbar" style="width: 87%" aria-valuenow="87" aria-valuemin="0" aria-valuemax="100"></div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="col-md-6 col-xl-3 mb-4">
                  <div class="card shadow border-0">
                    <div class="card-body">
                      <div class="row align-items-center">
                        <div class="col-3 text-center">
                          <span class="circle circle-sm bg-primary">
                            <i class="fe fe-16 fe-activity text-white mb-0"></i>
                          </span>
                        </div>
                        <div class="col">
                          <p class="small text-muted mb-0">AVG Orders</p>
                          <span class="h3 mb-0">$80</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="row align-items-center my-2">
                <div class="col-auto ml-auto">
                  <form class="form-inline">
                    <div class="form-group">
                      <label for="reportrange" class="sr-only">Date Ranges</label>
                      <div id="reportrange" class="px-2 py-2 text-muted">
                        <i class="fe fe-calendar fe-16 mx-2"></i>
                        <span class="small"></span>
                      </div>
                    </div>
                    <div class="form-group">
                      <button type="button" class="btn btn-sm"><span class="fe fe-refresh-ccw fe-12 text-muted"></span></button>
                      <button type="button" class="btn btn-sm"><span class="fe fe-filter fe-12 text-muted"></span></button>
                    </div>
                  </form>
                </div>
              </div>

              <div class="row my-4">
                <div class="col-md-12">
                  <div class="chart-box">
                    <div id="columnChart"></div>
                  </div>
                </div>
              </div>
              
              <div class="row">
                <div class="col-md-4">
                  <div class="card shadow mb-4">
                    <div class="card-body">
                      <div class="chart-widget">
                        <div id="gradientRadial"></div>
                      </div>
                      <div class="row">
                        <div class="col-6 text-center">
                          <p class="text-muted mb-0">Yesterday</p>
                          <h4 class="mb-1">126</h4>
                          <p class="text-muted mb-2">+5.5%</p>
                        </div>
                        <div class="col-6 text-center">
                          <p class="text-muted mb-0">Today</p>
                          <h4 class="mb-1">86</h4>
                          <p class="text-muted mb-2">-5.5%</p>
                        </div>
                      </div>
                    </div> 
                  </div> 
                </div> 
                <div class="col-md-4">
                  <div class="card shadow mb-4">
                    <div class="card-body">
                      <div class="chart-widget mb-2">
                        <div id="radialbar"></div>
                      </div>
                      <div class="row items-align-center">
                        <div class="col-4 text-center">
                          <p class="text-muted mb-1">Cost</p>
                          <h6 class="mb-1">$1,823</h6>
                          <p class="text-muted mb-0">+12%</p>
                        </div>
                        <div class="col-4 text-center">
                          <p class="text-muted mb-1">Revenue</p>
                          <h6 class="mb-1">$6,830</h6>
                          <p class="text-muted mb-0">+8%</p>
                        </div>
                        <div class="col-4 text-center">
                          <p class="text-muted mb-1">Earning</p>
                          <h6 class="mb-1">$4,830</h6>
                          <p class="text-muted mb-0">+8%</p>
                        </div>
                      </div>
                    </div> 
                  </div> 
                </div> 
                <div class="col-md-4">
                  <div class="card shadow mb-4">
                    <div class="card-body">
                      <p class="mb-0"><strong class="mb-0 text-uppercase text-muted">Today</strong></p>
                      <h3 class="mb-0">$2,562.30</h3>
                      <p class="text-muted">+18.9% Last week</p>
                      <div class="chart-box mt-n5">
                        <div id="lineChartWidget"></div>
                      </div>
                      <div class="row">
                        <div class="col-4 text-center mt-3">
                          <p class="mb-1 text-muted">Completions</p>
                          <h6 class="mb-0">26</h6>
                          <span class="small text-muted">+20%</span>
                          <span class="fe fe-arrow-up text-success fe-12"></span>
                        </div>
                        <div class="col-4 text-center mt-3">
                          <p class="mb-1 text-muted">Goal Value</p>
                          <h6 class="mb-0">$260</h6>
                          <span class="small text-muted">+6%</span>
                          <span class="fe fe-arrow-up text-success fe-12"></span>
                        </div>
                        <div class="col-4 text-center mt-3">
                          <p class="mb-1 text-muted">Conversion</p>
                          <h6 class="mb-0">6%</h6>
                          <span class="small text-muted">-2%</span>
                          <span class="fe fe-arrow-down text-danger fe-12"></span>
                        </div>
                      </div>
                    </div> 
                  </div> 
                </div> 
			  </div>
              <div class="row">

                <div class="col-md-12">
                  <h6 class="mb-3">orders</h6>
                  <table class="table table-borderless table-striped">
                    <thead>
                      <tr role="row">
                        <th>ID</th>
                        <th>Purchase Date</th>
                        <th>Name</th>
                        <th>Phone</th>
                        <th>Address</th>
                        <th>Total</th>
                        <th>Payment</th>
                        <th>Status</th>
                        <th>Action</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <th scope="col">1331</th>
                        <td>2020-12-26 01:32:21</td>
                        <td>Kasimir Lindsey</td>
                        <td>(697) 486-2101</td>
                        <td>996-3523 Et Ave</td>
                        <td>$3.64</td>
                        <td> Paypal</td>
                        <td>Shipped</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1156</th>
                        <td>2020-04-21 00:38:38</td>
                        <td>Melinda Levy</td>
                        <td>(748) 927-4423</td>
                        <td>Ap #516-8821 Vitae Street</td>
                        <td>$4.18</td>
                        <td> Paypal</td>
                        <td>Pending</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1038</th>
                        <td>2019-06-25 19:13:36</td>
                        <td>Aubrey Sweeney</td>
                        <td>(422) 405-2736</td>
                        <td>Ap #598-7581 Tellus Av.</td>
                        <td>$4.98</td>
                        <td>Credit Card </td>
                        <td>Processing</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1227</th>
                        <td>2021-01-22 13:28:00</td>
                        <td>Timon Bauer</td>
                        <td>(690) 965-1551</td>
                        <td>840-2188 Placerat, Rd.</td>
                        <td>$3.46</td>
                        <td> Paypal</td>
                        <td>Processing</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1956</th>
                        <td>2019-11-11 16:23:17</td>
                        <td>Kelly Barrera</td>
                        <td>(117) 625-6737</td>
                        <td>816 Ornare, Street</td>
                        <td>$4.16</td>
                        <td>Credit Card </td>
                        <td>Shipped</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1669</th>
                        <td>2021-04-12 07:07:13</td>
                        <td>Kellie Roach</td>
                        <td>(422) 748-1761</td>
                        <td>5432 A St.</td>
                        <td>$3.53</td>
                        <td> Paypal</td>
                        <td>Shipped</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                      <tr>
                        <th scope="col">1909</th>
                        <td>2020-05-14 00:23:11</td>
                        <td>Lani Diaz</td>
                        <td>(767) 486-2253</td>
                        <td>3328 Ut Street</td>
                        <td>$4.29</td>
                        <td> Paypal</td>
                        <td>Pending</td>
                        <td>
                          <div class="dropdown">
                            <button class="btn btn-sm dropdown-toggle more-vertical" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                              <span class="text-muted sr-only">Action</span>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                              <a class="dropdown-item" href="#">Edit</a>
                              <a class="dropdown-item" href="#">Remove</a>
                              <a class="dropdown-item" href="#">Assign</a>
                            </div>
                          </div>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div> 
              </div> 
            </div>
          </div>
        </div> 

    <script src="/js/tinycolor-min.js"></script>
    <script src="/js/config.js"></script>
    <script src="/js/apexcharts.min.js"></script>
    <script src="/js/apexcharts.custom.js"></script>
    <script>
      window.dataLayer = window.dataLayer || [];

      function gtag()
      {
        dataLayer.push(arguments);
      }
      gtag('js', new Date());
      gtag('config', 'UA-56159088-1');
    </script>
    -->
	<!--<div class="dashboard_wrap">
		<div class="dh_con c01">
			<div class="dc_tit01">프로젝트 개요</div>
			<div class="order_comment">
				ROCCAT KONE EMP 동부대우전자 중국법인 냉장고 부품 수입,광학 센서가 장착돼 보다 정밀하고 신속한 움직임을 지원하는 점이 특징으로, 해상운송 및 보관창고 14일 대기 필요이후 대응 하는게 좋을  것으로 봄
			</div>
		</div>
		<div class="dh_con c02">
			<div class="dc_tit01">진행현황</div>
			<ul class="state_list">
				<li>
					<p class="tit t01">견적</p>
					<p class="date">5월 16일</p>
				</li>
				<li>
					<p class="tit t02">수입대행</p>
					<p class="date">5월 16일</p>
				</li>
				<li>
					<p class="tit t03">국제운송</p>
					<p class="date">5월 16일</p>
				</li>
				<li>
					<p class="tit t04">통관</p>
					<p class="date">5월 16일</p>
				</li>
				<li>
					<p class="tit t04">국내운송</p>
					<p class="date">5월 16일</p>
				</li>
			</ul>
		</div>
		<div class="dh_con c03">
			<div class="dc_tit01"><span class="t_blue">동부대우전자 베트남 법인 LED 부품 수입</span> 통관 진행 DASH BOARD</div>
			<div class="dc_txt01">지금수입 진행 요청 하시면 국내운송까지 <span class="t_red">10일 소요 예정</span> 입니다. </div>

			<div class="dashboard">
				<ul class="dsh_list">
					<li>
						<div class="dsh_tit t01">견적</div>
						<ul class="dsh_box">
							<li class="on">
								<p class="tit"><span class="st01">완료</span> 견적요청</p>
								<p class="date">2020-02-01</p>
								<a href="" class="btn_cnt">+2</a>
							</li>
							<li class="on">
								<p class="tit"><span>완료</span> 견적요청</p>
								<p class="date">2020-02-01</p>
								<a href="" class="btn_cnt">+2</a>
							</li>
						</ul>
					</li>
					<li>
						<div class="dsh_tit t02">수입대행</div>
						<ul class="dsh_box">
							<li>
								<p class="tit">발주요청</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">발주요청</p>
								<p class="date">-</p>
							</li>
						</ul>
					</li>
					<li>
						<div class="dsh_tit t03">국제운송</div>
						<ul class="dsh_box">
							<li>
								<p class="tit">발주요청</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">발주요청</p>
								<p class="date">-</p>
							</li>
						</ul>
					</li>
					<li>
						<div class="dsh_tit t04">통관</div>
						<ul class="dsh_box v02">
							<li class="on">
								<p class="tit"><span class="st02">예정</span> 통관요청</p>
								<p class="date">2020-02-01</p>
								<a href="" class="btn_cnt">+2</a>
							</li>
							<li class="on">
								<p class="tit"><span class="st02">예정</span> 신고작성</p>
								<p class="date">2020-02-01</p>
								<a href="" class="btn_cnt">+2</a>
							</li>
							<li class="on">
								<p class="tit"><span class="st02">예정</span> 입항여부</p>
								<p class="date">2020-02-01</p>
								<a href="" class="btn_cnt">+2</a>
							</li>
							<li>
								<p class="tit">신고전송</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">신고심사</p>
								<p class="date">-</p>
							</li>
						</ul>
						<ul class="dsh_box v02">
							<li>
								<p class="tit">관세납부</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">수리완료</p>
								<p class="date">-</p>
							</li>
						</ul>
					</li>
					<li>
						<div class="dsh_tit t05">국내운송</div>
						<ul class="dsh_box">
							<li>
								<p class="tit">발주요청</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">화물운송</p>
								<p class="date">-</p>
							</li>
							<li>
								<p class="tit">화물인도</p>
								<p class="date">-</p>
							</li>
						</ul>
					</li>
				</ul>
			</div>
		</div>
		<div class="dh_con c04">
			<div class="dc_tit02">전체 코멘트</div>
			<div class="tblst1">
				<table summary="목록">                
					<colgroup>
						<col style="width:40%;"/>
						<col style="width:15%;"/>
						<col style="width:15%;"/>
						<col style="width:15%;"/>
						<col style="width:15%;"/>					
					</colgroup>
					<thead>
					<tr>
						<th>내용</th>
						<th>상태</th>
						<th>등록일</th>
						<th>작성자</th>
					</tr>                	
					</thead>
					<tbody>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					<tr>
						<td class="left"><a href="">Q 티엘링크 사용방법 설명 안내드립니다. <span class="t_org">(1)</span></a></td>
						<td class="center">답변완료</td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
					</tr>
					</tbody>
				</table>
			</div>
		</div>
		<div class="dh_con c05">
			<div class="dc_tit02">관련 문서</div>
			<div class="tblst1">
				<table summary="목록">                
					<colgroup>
						<col style="width:18%;"/>
						<col style="width:27%;"/>
						<col style="width:25%;"/>
						<col style="width:15%;"/>
						<col style="width:15%;"/>					
					</colgroup>
					<thead>
					<tr>
						<th>구분</th>
						<th>문서명</th>
						<th>등록일</th>
						<th>작성자</th>
						<th>첨부파일</th>                   
					</tr>                	
					</thead>
					<tbody>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					<tr>
						<td class="center">창고운임</td>
						<td class="left"><a href="" class="t_blue">창고운임 자료</a></td>
						<td class="center">2020-03-20 18:30</td>
						<td class="center">홍길동</td>
						<td class="center"><a href="" class="file"><img src="images/ic_file.png"></a></td>                    
					</tr>
					</tbody>
				</table>
			</div>
		</div>
	</div>-->
</asp:Content>
