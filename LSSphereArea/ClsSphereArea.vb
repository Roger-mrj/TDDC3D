
Imports System.Math
Imports ESRI.ArcGIS.Geometry

Namespace GGP
    Public Class ClsSphereArea

        '双精度类型：
        '圆周率值
        Private PI As Double = 3.14159265358979
        '中央经线
        Private CenterL As Double
        '
        Private RHO As Double = 206264.8062471

        Private ParamA As Double
        Private ParamB As Double
        Private ParamC As Double
        Private ParamD As Double
        Private ParamE As Double

        Const ZERO As Double = 0.000000000001

        '80椭球常数

        '椭球长半轴：
        Private aRadius As Double = 6378137  '6378140
        '椭球短半轴：
        Private bRadius As Double = 6356752.31414036  '6356755.29
        '椭球扁率：
        Private ParaAF As Double = 1 / 298.257222101

        '国土部给的偏心率一、二反了
        '椭球第二偏心率：
        Private ParaE2 As Double = 0.0066943800229   '0.00669438499958795 '6.69438499958795E-03
        '椭球第一偏心率：
        Private ParaE1 As Double = 0.00673949677548            '0.00673950181947292 '6.73950181947292E-03
        '极点子午圈曲率半径：
        Private ParaC As Double = 6399593.62586   '6399596.65198801

        'k0:
        Private Parak0 As Double = 0.000000157048761144159  '0.000000157048687472752 '1.57048687472752E-07
        'k1:
        Private Parak1 As Double = 0.00505250178820567   '0.00505250559291393 '5.05250559291393E-03
        'k2:
        Private Parak2 As Double = 0.0000298472900956587  '0.0000298473350966158 '2.98473350966158E-05
        'k3:
        Private Parak3 As Double = 0.000000241626669230084  '0.000000241627215981336 ' 2.41627215981336E-07
        'k4:
        Private Parak4 As Double = 0.00000000222241238938534 '0.00000000222241909461273 '2.22241909461273E-09
        '选定本初子午线为参考经线
        Private StandardLat As Double

        '度带号
        Private DDH As Double
        Private StatUnitLength As Integer
        Private StatUnit As String
        Private CenterX As Double = 0 '从坐标系统里获取的中央经线

        Property _StatUnitLength() As Integer
            Get

            End Get
            Set(ByVal value As Integer)
                StatUnitLength = value
            End Set
        End Property
        Property _StatUnit() As Integer
            Get

            End Get
            Set(ByVal value As Integer)
                StatUnit = value
            End Set
        End Property
        Public Function SphereArea(ByVal pGeometry As IGeometry, ByVal _DDH As Double, Optional ByVal ISuNIT As Boolean = False) As Double
            Dim pArea As Double = 0
            Dim pP4 As IPolygon4
            Dim pGC As IGeometryCollection
            Dim pGC1 As IGeometryCollection
            Dim j, k As Integer
            Dim pPC As IProjectedCoordinateSystem

            If Not pGeometry.SpatialReference Is Nothing Then
                If TypeOf pGeometry.SpatialReference Is IProjectedCoordinateSystem Then
                    pPC = pGeometry.SpatialReference
                    CenterX = pPC.CentralMeridian(True)
                End If
            End If
            '''''''''1-6按照张姐意思修改球面面积,修改人：聂震宇
            If CenterX = 0 Then
                CenterX = _DDH * 3
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''
            pP4 = pGeometry
            pP4.SimplifyPreserveFromTo()

            pGC = pP4.ExteriorRingBag()
            For j = 0 To pGC.GeometryCount - 1

                pArea -= GetSphereArea_1(pGC.Geometry(j), _DDH)

                pGC1 = pP4.InteriorRingBag(pGC.Geometry(j))
                For k = 0 To pGC1.GeometryCount - 1

                    pArea -= GetSphereArea_1(pGC1.Geometry(k), _DDH)

                Next
            Next
            pArea = System.Math.Round(pArea, 2, MidpointRounding.AwayFromZero)

            If Not ISuNIT Then
                pArea = GetUnitArea(pArea)
            End If
            Return pArea
        End Function
        Public Function SphereAreaTF(ByVal pGeometry As IGeometry, ByVal _DDH As Double) As Double
            Dim pArea As Double = 0
            Dim pP4 As IPolygon4
            Dim pGC As IGeometryCollection
            Dim pGC1 As IGeometryCollection
            Dim j, k As Integer

            pP4 = pGeometry
            pP4.SimplifyPreserveFromTo()

            pGC = pP4.ExteriorRingBag()
            For j = 0 To pGC.GeometryCount - 1

                pArea -= GetSphereArea_1(pGC.Geometry(j), _DDH)
                pGC1 = pP4.InteriorRingBag(pGC.Geometry(j))
                For k = 0 To pGC1.GeometryCount - 1
                    pArea -= GetSphereArea_1(pGC1.Geometry(k), _DDH)
                Next
            Next
            pArea = pArea
            Return pArea
        End Function


        Public Function GetUnitArea(ByVal pArea As Double) As Double
            Dim pDblmj As Double
            Select Case StatUnit
                Case "0"
                    pDblmj = System.Math.Round(pArea / 10000, StatUnitLength, MidpointRounding.AwayFromZero)
                Case "1"
                    pDblmj = System.Math.Round(pArea * 0.0015, StatUnitLength, MidpointRounding.AwayFromZero)
                Case "2"
                    pDblmj = System.Math.Round(pArea, StatUnitLength, MidpointRounding.AwayFromZero)
            End Select

            Return pDblmj
        End Function
       
        Public Function GetSphereArea_1(ByVal pGeometry As IGeometry, ByVal _DDH As Double, Optional ByVal ISuNIT As Boolean = False) As Double
            '初始化参数
            Dim e As Double
            Dim B As Double
            Dim L As Double
            Dim B1 As Double
            Dim L1 As Double
            Dim areaSum As Double
            Dim i As Integer
            Dim PntColl As IPointCollection

            DDH = _DDH

            '中央经线

            CenterL = CenterX



            e = ParaE2
            'ParaC = aRadius / (1 - ParaAF)

            ParamA = 1 + (3 / 6) * e + (30 / 80) * Pow(e, 2) + (35 / 112) * Pow(e, 3) + (630 / 2304) * Pow(e, 4)
            ParamB = (1 / 6) * e + (15 / 80) * Pow(e, 2) + (21 / 112) * Pow(e, 3) + (420 / 2304) * Pow(e, 4)
            ParamC = (3 / 80) * Pow(e, 2) + (7 / 112) * Pow(e, 3) + (180 / 2304) * Pow(e, 4)
            ParamD = (1 / 112) * Pow(e, 3) + (45 / 2304) * Pow(e, 4)
            ParamE = (5 / 2304) * Pow(e, 4)

            '中央经线转换为弧度
            CenterL = CenterL * PI / 180


            '选定本初子午线为参考经线
            StandardLat = 0

            PntColl = pGeometry

            For i = 0 To PntColl.PointCount - 2

                '由高斯坐标反解计算经纬度值()
                ComputeXYGeo(PntColl.Point(i).Y, PntColl.Point(i).X, B, L, CenterL)
                ComputeXYGeo(PntColl.Point(i + 1).Y, PntColl.Point(i + 1).X, B1, L1, CenterL)
                '将经纬度转换为弧度值()
                B = B / RHO
                L = L / RHO
                B1 = B1 / RHO
                L1 = L1 / RHO

                '计算梯形面积
                Dim AreaVal As Double ';//梯形面积值
                Dim lDiference As Double ';//经差
                Dim bDiference As Double '; //纬差
                Dim bSum As Double ';//纬度和
                Dim ItemValue(5) As Double ';//计算变量 

                bDiference = (B1 - B)

                bSum = (B1 + B) / 2

                lDiference = (L1 + L) / 2 - StandardLat

                ItemValue(0) = ParamA * Sin(bDiference / 2) * Cos(bSum)
                ItemValue(1) = ParamB * Sin(3 * bDiference / 2) * Cos(3 * bSum)
                ItemValue(2) = ParamC * Sin(5 * bDiference / 2) * Cos(5 * bSum)
                ItemValue(3) = ParamD * Sin(7 * bDiference / 2) * Cos(7 * bSum)
                ItemValue(4) = ParamE * Sin(9 * bDiference / 2) * Cos(9 * bSum)
                AreaVal = 2 * bRadius * lDiference * bRadius * (ItemValue(0) - ItemValue(1) + ItemValue(2) - ItemValue(3) + ItemValue(4))

                areaSum = areaSum + AreaVal
            Next

            Return areaSum

        End Function

#Region "高斯坐标反解算法"
        '高斯坐标反解算法
        Public Sub ComputeXYGeo(ByVal x As Double, ByVal y As Double, ByRef B As Double, ByRef L As Double, ByVal center As Double)

            Dim y1 As Double
            Dim bf As Double

            Dim i As Integer
            Dim str As String

            i = Microsoft.VisualBasic.InStr(CStr(y), ".", Microsoft.VisualBasic.CompareMethod.Text)
            If i = 0 Then
                str = y
            Else
                str = Microsoft.VisualBasic.Left(y, i - 1)
            End If

            If str.Length = 8 Then
                y1 = y - 500000 - DDH * 1000000
            Else
                y1 = y - 500000
            End If


            Dim e As Double

            e = Parak0 * x

            Dim se As Double

            se = Sin(e)
            bf = e + Cos(e) * (Parak1 * se - Parak2 * Pow(se, 3) + Parak3 * Pow(se, 5) - Parak4 * Pow(se, 7))

            Dim v As Double
            Dim t As Double
            Dim N As Double
            Dim nl As Double
            Dim vt As Double
            Dim yn As Double
            Dim t2 As Double
            Dim g As Double

            g = 1

            t = Tan(bf)
            nl = ParaE1 * Pow(Cos(bf), 2)
            v = Sqrt(1 + nl)
            N = ParaC / v
            yn = y1 / N
            vt = Pow(v, 2) * t
            t2 = Pow(t, 2)
            B = bf - vt * Pow(yn, 2) / 2 + (5 + 3 * t2 + nl - 9 * nl * t2) * vt * Pow(yn, 4) / 24 - (61 + 90 * t2 + 45 * Pow(t2, 2)) * vt * Pow(yn, 6) / 720

            B = TransArcToDegree(B)

            Dim cbf As Double

            cbf = 1 / Cos(bf)
            L = cbf * yn - (1 + 2 * t2 + nl) * cbf * Pow(yn, 3) / 6 + (5 + 28 * t2 + 24 * Pow(t2, 2) + 6 * nl + 8 * nl * t2) * cbf * Pow(yn, 5) / 120 + center
            L = TransArcToDegree(L)
        End Sub


#Region "弧度转换为度"
        Public Function TransArcToDegree(ByVal arc As Double) As Double
            Dim degree As Double
            Dim min As Double
            Dim sec As Double
            Dim ret As Double
            Dim tmp As Double
            ret = arc * 180 / PI
            degree = FormatValue(ret, 100, 100)
            tmp = (ret - degree) * 60
            min = FormatValue(tmp, 100, 100)
            sec = (tmp - min) * 60
            '秒保留到小数点后6位，四舍五入
            sec = Microsoft.VisualBasic.Format(sec, "####.000000") 'FormatValue(sec, 10000000, 100)
            TransArcToDegree = degree * 3600 + min * 60 + sec
        End Function

        Private Function FormatValue(ByVal inputVal As Double, ByVal precsion As Long, ByVal scaleNum As Long) As Double
            FormatValue = (Microsoft.VisualBasic.Int(inputVal * precsion) - Microsoft.VisualBasic.Int(inputVal * precsion) Mod scaleNum) / precsion
        End Function

#End Region
#End Region

#Region "获取参考经线"
        '高斯坐标反解算法
        Public Function GetJX(ByVal x As Double, ByVal y As Double, ByRef L As Double, ByVal center As Double) As Double

            Dim y1 As Double
            Dim bf As Double

            Dim i As Integer
            Dim str As String

            i = Microsoft.VisualBasic.InStr(CStr(y), ".", Microsoft.VisualBasic.CompareMethod.Text)
            If i = 0 Then
                str = y
            Else
                str = Microsoft.VisualBasic.Left(y, i - 1)
            End If

            'If str.Length = 8 Then
            y1 = y - 500000 - DDH * 1000000
            'Else
            '    y1 = y - 500000
            'End If


            Dim e As Double

            e = Parak0 * x

            Dim se As Double

            se = Sin(e)
            bf = e + Cos(e) * (Parak1 * se - Parak2 * Pow(se, 3) + Parak3 * Pow(se, 5) - Parak4 * Pow(se, 7))

            Dim v As Double
            Dim t As Double
            Dim N As Double
            Dim nl As Double
            Dim vt As Double
            Dim yn As Double
            Dim t2 As Double
            Dim g As Double

            g = 1

            t = Tan(bf)
            nl = ParaE1 * Pow(Cos(bf), 2)
            v = Sqrt(1 + nl)
            N = ParaC / v
            yn = y1 / N
            vt = Pow(v, 2) * t
            t2 = Pow(t, 2)


            Dim cbf As Double

            cbf = 1 / Cos(bf)
            L = cbf * yn - (1 + 2 * t2 + nl) * cbf * Pow(yn, 3) / 6 + (5 + 28 * t2 + 24 * Pow(t2, 2) + 6 * nl + 8 * nl * t2) * cbf * Pow(yn, 5) / 120 + center

        End Function
#End Region


    End Class
End Namespace


