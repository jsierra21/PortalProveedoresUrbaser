USE [PortalProveedoresDB]
GO
/****** Object:  StoredProcedure [dbo].[SpConsultaHistoricoPagosApoteosys]    Script Date: 10/18/2023 9:31:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--- ===============================================================
--- Author: <CARLOS ALCALA>
--- Create date: <09/09/2022>
--- EXEC [dbo].[SpConsultaHistoricoPagosApoteosys]
--- Description: CONSULTA HISTORIA DE PAGOS Y DESCUENTOS DE PROVEEDOR
--- ===============================================================

ALTER PROC [dbo].[SpConsultaHistoricoPagosApoteosys]
@Operacion			INT = 1
,@FechaInicial		VARCHAR(50)	= '20/10/2022'
,@FechaFinal		VARCHAR(50)	= '15/12/2022'
,@Empresa			VARCHAR(20)	= 'NSGS'
,@NitProveedor		VARCHAR(50)	= '900379269'
,@TipoDocCausacion	VARCHAR(20)	= 'ET04'
,@NumDocCausacion	VARCHAR(20)	= '2421'

--,@FechaInicial		VARCHAR(50)	= '01/01/2022'
--,@FechaFinal		VARCHAR(50)	= '08/09/2022'
--,@Empresa			VARCHAR(20)	= 'NAIS'
--,@NitProveedor		VARCHAR(50)	= '900600065'
--,@TipoDocCausacion	VARCHAR(20)	= 'ET13'
--,@NumDocCausacion	VARCHAR(20)	= '994'

-- ,@FechaInicial		VARCHAR(50)	= '01/01/2022'
-- ,@FechaFinal		VARCHAR(50)	= '08/09/2022'
-- ,@Empresa			VARCHAR(20)	= 'NSGS'
-- ,@NitProveedor		VARCHAR(50)	= '900600065'
-- ,@TipoDocCausacion	VARCHAR(20)	= 'ET05'
-- ,@NumDocCausacion	VARCHAR(20)	= '1106'

AS 

SET LANGUAGE 'SPANISH'
SET NOCOUNT ON

BEGIN TRY
    --------------------------------------------------------------------------------------------------
    -- Declaración de variables
    --------------------------------------------------------------------------------------------------
	DECLARE @storedProcedureName VARCHAR(MAX)
    SELECT @storedProcedureName = OBJECT_NAME(@@PROCID)

	DECLARE @strSQL1 NVARCHAR(MAX),
			@strSQL2 NVARCHAR(MAX)

	DECLARE @cntmax INT = 0,
			@cnt INT = 1,
			@TC VARCHAR(20),
			@NC VARCHAR(20),
			@TDA VARCHAR(20),
			@NDA VARCHAR(20),
			@TDC VARCHAR(20),
			@NDC VARCHAR(20),
			@AnioCausacion INT,
			@PeriodoCausacion INT


	--DECLARE @strString NVARCHAR(4000) 
	--DECLARE @respuesta NVARCHAR(4000)

	DECLARE @PathGetFactPagas NVARCHAR(MAX),
			@PathGetRetFactPagas NVARCHAR(MAX),
            @PathApiPortal NVARCHAR(MAX),
            @PathAuth NVARCHAR(MAX),
			@body_ NVARCHAR(MAX)

	DECLARE @ResponseText AS NVARCHAR(MAX),
            @apiResponse NVARCHAR(MAX),
            @RecibirErrorMessage NVARCHAR(MAX),
            -- @ReturnSpErrorMessage NVARCHAR(MAX),
            @msg AS NVARCHAR(MAX),
            @authHeader AS NVARCHAR(MAX),
            @tokenAdb AS VARCHAR(MAX),
            @statusText NVARCHAR(32),
            @status INT,
            @resp INT = 0
    --------------------------------------------------------------------------------------------------
    -- ajuste de variables
    --------------------------------------------------------------------------------------------------

    --------------------------------------------------------------------------------------------------
    -- Validación existencia de tablas temporales
    --------------------------------------------------------------------------------------------------	
    IF OBJECT_ID('tempdb..#pagos') IS NOT NULL BEGIN
        DROP TABLE #pagos
    END			

    IF OBJECT_ID('tempdb..#controlCausaciones') IS NOT NULL BEGIN
        DROP TABLE #controlCausaciones
    END				

    IF OBJECT_ID('tempdb..#causaciones') IS NOT NULL BEGIN
        DROP TABLE #causaciones
    END			
    
    IF OBJECT_ID('tempdb..#retenciones') IS NOT NULL BEGIN
        DROP TABLE #retenciones
    END			
    
    CREATE TABLE #pagos (
        Id							INT IDENTITY(1,1),
        Tipo_Movimiento				CHAR(1),
        Empresa						VARCHAR(50),
        NombreEmpresa				VARCHAR(150),			
        Anio						INT,
        Periodo						INT,
        Fecha						VARCHAR(30),
        Codigo_Tercero				VARCHAR(50),			
        Nombre_Tercero				VARCHAR(150),
        Cuenta						VARCHAR(50),
        Nombre_Cuenta_Con			VARCHAR(150),
        Cuenta_Consignacion			VARCHAR(100),
        Nombre_Banco				VARCHAR(100),
        Tipo_Cuenta_Banco			VARCHAR(100),
        Nombre_Cuenta				VARCHAR(100),
        Tipo_Documento_Causacion	VARCHAR(20),
        Numero_Documento_Causacion	VARCHAR(20),
        Tipo_Documento_Alterno		VARCHAR(20),
        Numero_Documento_Alterno	VARCHAR(20),
        Observaciones				VARCHAR(250),
        Base_Retencion				DECIMAL(18,2),
        Debito						DECIMAL(18,2),
        Credito						DECIMAL(18,2)
    ) 
    
	CREATE TABLE #controlCausaciones (
        Tipo_Documento_Causacion	VARCHAR(20),
        Numero_Documento_Causacion	VARCHAR(20),
		Tipo_Documento_Alterno		VARCHAR(20),
        Numero_Documento_Alterno	VARCHAR(20),
		Anio						INT,
        Periodo						INT
	)

    CREATE TABLE #causaciones (
        Id							INT IDENTITY(1,1),
        Tipo_Movimiento				CHAR(1),
        Empresa						VARCHAR(50),
        NombreEmpresa				VARCHAR(150),			
        Anio						INT,
        Periodo						INT,
        Fecha						VARCHAR(30),
        Codigo_Tercero				VARCHAR(50),			
        Nombre_Tercero				VARCHAR(150),
        Cuenta						VARCHAR(50),
        Nombre_Cuenta_Con			VARCHAR(150),
        Cuenta_Consignacion			VARCHAR(100),
        Nombre_Banco				VARCHAR(100),
        Tipo_Cuenta_Banco			VARCHAR(100),
        Nombre_Cuenta				VARCHAR(100),
        Tipo_Documento_Causacion	VARCHAR(20),
        Numero_Documento_Causacion	VARCHAR(20),
        Tipo_Documento_Alterno		VARCHAR(20),
        Numero_Documento_Alterno	VARCHAR(20),
        Observaciones				VARCHAR(250),
        Base_Retencion				DECIMAL(18,2),
        Debito						DECIMAL(18,2),
        Credito						DECIMAL(18,2)
    ) 
    
    CREATE TABLE #retenciones (
        Id							INT IDENTITY(1,1),
        Tipo_Movimiento				CHAR(1),
        Empresa						VARCHAR(50),
        NombreEmpresa				VARCHAR(150),			
        Anio						INT,
        Periodo						INT,
        Fecha						VARCHAR(30),
        Codigo_Tercero				VARCHAR(50),			
        Nombre_Tercero				VARCHAR(150),
        Cuenta						VARCHAR(50),
        Nombre_Cuenta_Con			VARCHAR(150),
        Cuenta_Consignacion			VARCHAR(100),
        Nombre_Banco				VARCHAR(100),
        Tipo_Cuenta_Banco			VARCHAR(100),
        Nombre_Cuenta				VARCHAR(100),
        Tipo_Documento_Causacion	VARCHAR(20),
        Numero_Documento_Causacion	VARCHAR(20),
        Tipo_Documento_Alterno		VARCHAR(20),
        Numero_Documento_Alterno	VARCHAR(20),
        Observaciones				VARCHAR(250),
        Base_Retencion				DECIMAL(18,2),
        Debito						DECIMAL(18,2),
        Credito						DECIMAL(18,2),
        Tdc							VARCHAR(20),
        Ndc							VARCHAR(20),
    ) 

	IF @Operacion = 1 BEGIN -- Obtenemos Facturas Pagadas con sus repectivas Retenciones
		print 'inicia select1 - insert de apo a sql ' + CONVERT(VARCHAR(30),GETDATE())
		SET @strSQL1 = N' 
    
		SELECT  * FROM OPENQUERY (Apoteosys,
		''SELECT ''''P'''' TIPO_MOVIMIENTO, MC_____CODIGO____CONTAB_B Empresa,
				CASE
					WHEN MC_____CODIGO____CONTAB_B = ''''NSGS'''' THEN ''''URBASER COLOMBIA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSTJ'''' THEN ''''URBASER TUNJA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NCAF'''' THEN ''''URBASER MONTENEGRO S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NAIS'''' THEN ''''URBASER SOACHA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAD'''' THEN ''''URBASER DUITAMA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAT'''' THEN ''''URBASER LA TEBAIDA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAP'''' THEN ''''URBASER POPAYAN S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NCAU'''' THEN ''''URBASER ECOAMBIENTAL S.A. E.S.P.''''
					ELSE ''''''''
				END AS NombreEmpresa,
				MC_____CODIGO____PF_____B Anio,
				MC_____NUMERO____PERIOD_B Periodo,
				TO_CHAR(MC_____FECHA_____B, ''''DD/MM/YYYY'''') Fecha,
				MC_____IDENTIFIC_TERCER_B Codigo_Tercero,
				t.TERCER_NOMBEXTE__B Nombre_Tercero,
				MC_____CODIGO____CPC____B Cuenta,
				p.CPC____NOMBRE____B Nombre_cuenta_con,
				c.TCBM___NUMECUEN__B Cuenta_Consignacion,
				b.EF_____NOMBRE____B Nombre_Banco,
				CASE c.TCBM___TIPOCUEN__B
					WHEN 1 THEN ''''Corriente''''
					WHEN 2 THEN ''''Ahorros''''
				END AS Tipo_Cuenta_Banco,
				MC_____CODIGO____DS_____B||'''' ''''||MC_____NUMDOCSOP_B Nombre_Cuenta,
				MC_____CODIGO____TD_____B Tipo_Documento_Causacion,
				MC_____NUMERO____B Numero_Documento_Causacion,
				MC_____CODIGO____DS_____B Tipo_Documento_Alterno,
				MC_____NUMDOCSOP_B Numero_Documento_Alterno,
				MC_____OBSERVACI_B Observaciones,
				MC_____BASE______B Base_Retencion,
				MC_____DEBMONLOC_B Debito,
				MC_____CREMONLOC_B Credito
		FROM APOTEOSYS.MC____ a
		INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B
		INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = ''''S''''
		INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B
		INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = ''''NIIF''''
		WHERE a.MC_____CODIGO____CONTAB_B = ''''' + @Empresa + '''''
			AND MC_____CODIGO____TD_____B LIKE ''''ET%''''
		--  AND MC_____NUMERO____B = ''''128''''
			AND a.MC_____IDENTIFIC_TERCER_B = ''''' + @NitProveedor + '''''
		--  AND a.MC_____NUMDOCSOP_B = ''''SOFE675''''
			AND a.MC_____CODIGO____CONTAB_B != ''''AQSA''''
			AND a.MC_____CODIGO____CU_____B IS NOT NULL
			AND a.MC_____CODIGO____TD_____B != ''''CA''''
			AND a.MC_____FECHA_____B BETWEEN TO_DATE(''''' + @FechaInicial  + ''''',''''dd/mm/yyyy'''') 
			AND TO_DATE(''''' + @FechaFinal  + ''''',''''dd/mm/yyyy'''')  
			AND a.MC_____DEBMONLOC_B > 0
			ORDER BY MC_____FECHA_____B,
					MC_____CODIGO____TD_____B,
					MC_____NUMDOCSOP_B
		'' )'

		/* Query complementario de @strSQL1
		
			--SUM(MC_____BASE______B) Base_Retencion,
			--SUM(MC_____DEBMONLOC_B) Debito,
			--SUM(MC_____CREMONLOC_B) Credito,
			--SUM(MC_____DEBMONLOC_B) TotalDebito

			--GROUP BY MC_____CODIGO____CONTAB_B, 
			--        MC_____CODIGO____PF_____B,
			--        MC_____NUMERO____PERIOD_B,
			--        MC_____FECHA_____B,
			--        MC_____IDENTIFIC_TERCER_B,
			--        t.TERCER_NOMBEXTE__B,
			--        MC_____CODIGO____CPC____B,
			--        p.CPC____NOMBRE____B,
			--        TERCER_NUMECUEN__B,
			--        b.EF_____NOMBRE____B,
			--        MC_____CODIGO____DS_____B,
			--        MC_____NUMDOCSOP_B,
			--        MC_____CODIGO____TD_____B,
			--        MC_____NUMERO____B,
			--        MC_____OBSERVACI_B
		*/

		--print @strSQL1
		INSERT INTO #pagos
		EXEC SP_EXECUTESQL @strSQL1
    
		--SELECT TIPO_MOVIMIENTO,	EMPRESA,				NOMBREEMPRESA,		ANIO,
		--	   PERIODO,			FECHA,					CODIGOTERCERO,		NOMBRETERCERO,
		--	   CUENTA,			NOMBRE_CUENTA_CON,		CUENTA_CONSIGNACION,	NOMBRE_BANCO,		NOMBRE_CUENTA,
		--	   TIPO_DOCUMENTO_CAUSACION,					NUMERO_CAUSACION,	TIPO_DOCUMENTO_ALTERNO,
		--	   NUMERO_DOCUMENTO_ALTERNO,					OBSERVACIONES,		SUM(BASE_RETENCION),
		--	   SUM(DEBITO),			SUM(CREDITO),				SUM(TOTALDEBITO),		SUM(ISTOTAL)
		--FROM #pagos
		--GROUP BY TIPO_MOVIMIENTO,	EMPRESA,				NOMBREEMPRESA,		ANIO,
		--	   PERIODO,			FECHA,					CODIGOTERCERO,		NOMBRETERCERO,
		--	   CUENTA,			NOMBRE_CUENTA_CON,		CUENTA_CONSIGNACION,	NOMBRE_BANCO,		NOMBRE_CUENTA,
		--	   TIPO_DOCUMENTO_CAUSACION,					NUMERO_CAUSACION,	TIPO_DOCUMENTO_ALTERNO,
		--	   NUMERO_DOCUMENTO_ALTERNO,					OBSERVACIONES
    
		print 'inicia select - insert de apo a sql ' + CONVERT(VARCHAR(30),GETDATE())
		SET @strSQL2 = N' 
    
		SELECT  * FROM OPENQUERY (Apoteosys,
		''SELECT ''''C'''' TIPO_MOVIMIENTO, MC_____CODIGO____CONTAB_B Empresa,
				CASE
					WHEN MC_____CODIGO____CONTAB_B = ''''NSGS'''' THEN ''''URBASER COLOMBIA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSTJ'''' THEN ''''URBASER TUNJA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NCAF'''' THEN ''''URBASER MONTENEGRO S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NAIS'''' THEN ''''URBASER SOACHA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAD'''' THEN ''''URBASER DUITAMA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAT'''' THEN ''''URBASER LA TEBAIDA S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NSAP'''' THEN ''''URBASER POPAYAN S.A. E.S.P.''''
					WHEN MC_____CODIGO____CONTAB_B = ''''NCAU'''' THEN ''''URBASER ECOAMBIENTAL S.A. E.S.P.''''
					ELSE ''''''''
				END AS NombreEmpresa,
				MC_____CODIGO____PF_____B Anio,
				MC_____NUMERO____PERIOD_B Periodo,
				TO_CHAR(MC_____FECHA_____B, ''''DD/MM/YYYY'''') Fecha,
				MC_____IDENTIFIC_TERCER_B Codigo_Tercero,
				t.TERCER_NOMBEXTE__B Nombre_Tercero,
				MC_____CODIGO____CPC____B Cuenta,
				p.CPC____NOMBRE____B Nombre_cuenta_con,
				c.TCBM___NUMECUEN__B Cuenta_Consignacion,
				b.EF_____NOMBRE____B Nombre_Banco,
				CASE c.TCBM___TIPOCUEN__B
					WHEN 1 THEN ''''Corriente''''
					WHEN 2 THEN ''''Ahorros''''
				END AS Tipo_Cuenta_Banco,	
				MC_____CODIGO____DS_____B||'''' ''''||MC_____NUMDOCSOP_B Nombre_Cuenta,
				MC_____CODIGO____TD_____B Tipo_Documento_Causacion,
				MC_____NUMERO____B Numero_Documento_Causacion,
				MC_____CODIGO____DS_____B Tipo_Documento_Alterno,
				MC_____NUMDOCSOP_B Numero_Documento_Alterno,
				MC_____OBSERVACI_B Observaciones,
				MC_____BASE______B Base_Retencion,
				MC_____DEBMONLOC_B Debito,
				MC_____CREMONLOC_B Credito
		FROM APOTEOSYS.MC____ a
		INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B
		INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = ''''S''''
		INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B
		INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = ''''NIIF''''
		WHERE a.MC_____CODIGO____CONTAB_B = ''''' + @Empresa + '''''
			AND MC_____CODIGO____TD_____B LIKE ''''F%''''
			AND a.MC_____IDENTIFIC_TERCER_B = ''''' + @NitProveedor + '''''
			AND a.MC_____CODIGO____CONTAB_B != ''''AQSA''''
			AND a.MC_____CODIGO____CU_____B IS NOT NULL
			AND a.MC_____CODIGO____TD_____B != ''''CA''''
		ORDER BY MC_____FECHA_____B,
					MC_____CODIGO____TD_____B,
					MC_____NUMDOCSOP_B
		'' )'

		-- print @strSQL2
		INSERT INTO #causaciones
		EXEC SP_EXECUTESQL @strSQL2

		INSERT INTO #controlCausaciones (Tipo_Documento_Causacion,Numero_Documento_Causacion,Tipo_Documento_Alterno, Numero_Documento_Alterno, Anio, Periodo)
		SELECT DISTINCT
			b.Tipo_Documento_Causacion,
			b.Numero_Documento_Causacion,
			b.Tipo_Documento_Alterno,
			b.Numero_Documento_Alterno,
			b.Anio,
			b.Periodo
		FROM #pagos a
			INNER JOIN (
				SELECT 
					*, 
					ROW_NUMBER() OVER(PARTITION BY Anio, Periodo, Tipo_Documento_Alterno, Numero_Documento_Alterno ORDER BY Anio, Periodo, Tipo_Documento_Alterno, Numero_Documento_Alterno) AS RowNumer
				FROM #causaciones
			) AS b ON a.Tipo_Documento_Alterno = b.Tipo_Documento_Alterno AND a.Numero_Documento_Alterno = b.Numero_Documento_Alterno AND b.RowNumer = 1
		WHERE a.Tipo_Documento_Causacion = @TipoDocCausacion AND a.Numero_Documento_Causacion = @NumDocCausacion

		--select * from #controlCausaciones
		--SELECT * FROM #pagos
		--SELECT * FROM #causaciones
    
		SET @cntmax = (SELECT COUNT(*) FROM #pagos)
		SET @cnt = 1
    
		WHILE @cnt <= @cntmax BEGIN
			SET @TC  = NULL
			SET @NC  = NULL
			SET @TDA = NULL
			SET @NDA = NULL
			SET @TDC = NULL
			SET @NDC = NULL
                    
			--Capturamos documentos de pago
			SELECT
				@TC  = Tipo_Documento_Causacion,
				@NC  = Numero_Documento_Causacion,
				@TDA = Tipo_Documento_Alterno,
				@NDA = Numero_Documento_Alterno
			FROM #pagos
			WHERE id = @cnt	  
    
			PRINT CONCAT('ALTERNO: ', @TDA, ' - ', @NDA)
    
			--Buscamos documentos de causación
			SELECT
				@TDC = Tipo_Documento_Causacion,
				@NDC = Numero_Documento_Causacion
			FROM #causaciones
			WHERE Tipo_Documento_Alterno = @TDA 
			AND Numero_Documento_Alterno = @NDA
    
			PRINT CONCAT('CAUSACION: ', @TDC, ' - ', @NDC)

			-- Buscamos el periodo correspondiente de la factura
			SELECT 
				@AnioCausacion = Anio,
				@PeriodoCausacion = Periodo
			FROM #controlCausaciones
			WHERE Tipo_Documento_Causacion = @TDC
			AND Numero_Documento_Causacion = @NDC
			AND Tipo_Documento_Alterno = @TDA
			AND Numero_Documento_Alterno = @NDA
    
			--Registramos retenciones
			INSERT INTO #retenciones (	
				Tipo_Movimiento,			Empresa,					NombreEmpresa,				Anio,
				Periodo,					Fecha,						Codigo_Tercero,				Nombre_Tercero,
				Cuenta,						Nombre_Cuenta_Con,			Cuenta_Consignacion,		Nombre_Banco,
				Tipo_Cuenta_Banco,			Nombre_Cuenta,				Tipo_Documento_Causacion,	Numero_Documento_Causacion,		
				Tipo_Documento_Alterno,		Numero_Documento_Alterno,	Observaciones,				Base_Retencion,			
				Debito,						Credito,					Tdc,						Ndc
			)
			SELECT 
				'R' Tipo_Movimiento,					a.Empresa,								a.NombreEmpresa,				a.Anio,
				a.Periodo,								a.Fecha,								a.Codigo_Tercero,				a.Nombre_Tercero,
				a.Cuenta,								a.Nombre_Cuenta_Con,					a.Cuenta_Consignacion,			a.Nombre_Banco,	
				a.Tipo_Cuenta_Banco,					CONCAT(@TDA,' ',@NDA) Nombre_Cuenta,	a.Tipo_Documento_Causacion,		a.Numero_Documento_Causacion,			
				@TDA Tipo_Documento_Alterno,			@NDA Numero_Documento_Alterno,			a.Observaciones,				(a.Base_Retencion),			
				(a.Debito),								(a.Credito),							@TC,							@NC
			FROM #causaciones a
				LEFT JOIN #retenciones b ON b.Tipo_Documento_Causacion = a.Tipo_Documento_Causacion AND b.Numero_Documento_Causacion = a.Numero_Documento_Causacion
			WHERE a.Tipo_Documento_Causacion = @TDC
				AND a.Numero_Documento_Causacion = @NDC
				AND a.Anio = @AnioCausacion
				AND a.Periodo = @PeriodoCausacion
				AND a.Base_Retencion > 0
				AND b.Tipo_Documento_Causacion IS NULL
				AND b.Numero_Documento_Causacion IS NULL

			SET @cnt = @cnt + 1;
			--BREAK;
		END

		--select * from #retenciones
    
		SELECT 
			* 
		FROM (
			SELECT 
				Id,														Tipo_Movimiento,						Empresa,								NombreEmpresa,				
				Anio,													Periodo,								Fecha,									Codigo_Tercero,				
				Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
				Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
				Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
				CAST(FORMAT(Base_Retencion,'C','En-Us') AS VARCHAR(100)) Base_Retencion,	
				CAST(FORMAT(Debito,'C','En-Us') AS VARCHAR(100)) Debito,	
				CAST(FORMAT(Credito,'C','En-Us') AS VARCHAR(100)) Credito,										NULL Tdc,								NULL Ndc,
				CONVERT(VARCHAR(100),
					FORMAT(
						SUM(Debito) OVER(PARTITION BY Tipo_Documento_Causacion, Numero_Documento_Causacion ORDER BY Tipo_Documento_Causacion, Numero_Documento_Causacion DESC),
					'C','En-Us'
					)
				) AS Total_Movimiento
			FROM #pagos
			WHERE Tipo_Documento_Causacion = @TipoDocCausacion AND Numero_Documento_Causacion = @NumDocCausacion
			UNION ALL
            SELECT 
                Id,                                                     Tipo_Movimiento,						Empresa,								NombreEmpresa,				
                Anio,													Periodo,								Fecha,									Codigo_Tercero,				
                Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
                Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
                Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
                CAST(FORMAT(Base_Retencion,'C','En-Us') AS VARCHAR(100)) Base_Retencion,	                     
                CAST(FORMAT(Debito,'C','En-Us') AS VARCHAR(100)) Debito,
                CAST(FORMAT(Credito,'C','En-Us') AS VARCHAR(100)) Credito,                                      Tdc,									Ndc,
                CONVERT(VARCHAR(100),
                    FORMAT(
                        SUM(Credito) OVER(PARTITION BY Tipo_Documento_Causacion, Numero_Documento_Causacion ORDER BY Tipo_Documento_Causacion, Numero_Documento_Causacion DESC),
                        'C','En-Us'
                    )
                ) AS Total_Movimiento
            FROM (
                SELECT 
                    NULL Id,												Tipo_Movimiento,						Empresa,								NombreEmpresa,				
                    Anio,													Periodo,								Fecha,									Codigo_Tercero,				
                    Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
                    Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
                    Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
                    SUM(Base_Retencion) Base_Retencion,	                    SUM(Debito) Debito,	                    SUM(Credito) Credito,					Tdc,									
                    Ndc                                            
                FROM #retenciones
                WHERE Tdc = @TipoDocCausacion AND Ndc = @NumDocCausacion
                GROUP BY                                                Tipo_Movimiento,						Empresa,								NombreEmpresa,				
				Anio,													Periodo,								Fecha,									Codigo_Tercero,				
				Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
				Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
				Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,
                Tdc,                                                    Ndc
            ) AS tbretenciones
		) AS tb1
		ORDER BY Tipo_Movimiento, Id ASC
	END
	ELSE IF @Operacion = 2 BEGIN -- Obtenemos Facturas Pagadas con sus repectivas Retenciones, a través de API
		---//->) Se define la url de la api de Adobe, donde se verificará los documentos
		SELECT @PathApiPortal = [Path] FROM [conf].[PathsPortal] WHERE ID = 5
		--SET @PathAuth = CONCAT(@PathApiPortal, '/Usuario/Authentication')
		SET @PathGetFactPagas = CONCAT(@PathApiPortal, CONCAT('/Apoteosys/SearchFacturasPagadas_SQL?empresa=',@Empresa,'&nitProveedor=',@NitProveedor,'&fecha_Inicial=',@FechaInicial,'&fecha_Final=',@FechaFinal))
		SET @PathGetRetFactPagas = CONCAT(@PathApiPortal, CONCAT('/Apoteosys/SearchRetencionFacturasPagadas_SQL?empresa=',@Empresa,'&nitProveedor=',@NitProveedor))

		-- Obtenemos facturas pagadas
        EXEC @resp = api.SpConsumoApiSQL
        @Url = @PathGetFactPagas,
        @metodo = 'GET',
        @hasBody = 0,
        @isAuth = 0,
        @authIsRequired = 0,
		--@authorization = @authHeader,
        @contentType = 'application/json',
        @nombreSp = @storedProcedureName,
        @statusText = @statusText OUTPUT,
        @status = @status OUTPUT,
        @authHeader = @authHeader OUTPUT,
        @apiResponse = @ResponseText OUTPUT,
        @ReturnSpErrorMessage = @RecibirErrorMessage OUTPUT

		IF @resp = 0 BEGIN
			--SELECT @ResponseText
			IF(@ResponseText IS NOT NULL) BEGIN
                IF @status = 200 BEGIN
					INSERT INTO [log].[LogControlApi]
                    SELECT 		 
                        CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
                        , @storedProcedureName [Lca_NombreApi]
                        , @PathGetFactPagas [Lca_UrlApi]
                        , @statusText [Lca_TipoError]
                        , 'OPENJSON' [Lca_PackageName]
                        , 1 [Lca_Status]
                        , @ResponseText AS [Lca_MensajeEjecucion]
                        , GETDATE() [FechaRegistro]
						
					-- Llenamos tabla #pagos
					INSERT INTO #pagos
					SELECT 
						'P' Tipo_Movimiento,
						* 
					FROM OPENJSON(@ResponseText,'$.data')
					WITH (
						empresa						varchar(50)		'$.empresa',
						nombreEmpresa				varchar(150)	'$.nombreEmpresa',			
						anio						int				'$.anio',
						periodo						int				'$.periodo',
						fecha						varchar(30)		'$.fecha',
						codigo_Tercero				varchar(50)		'$.codigo_Tercero',			
						nombre_Tercero				varchar(150)	'$.nombre_Tercero',
						cuenta						varchar(50)		'$.cuenta',
						nombre_Cuenta_Con			varchar(150)	'$.nombre_Cuenta_Con',
						cuenta_Consignacion			varchar(100)	'$.cuenta_Consignacion',
						nombre_Banco				varchar(100)	'$.nombre_Banco',
						tipo_Cuenta_Banco			varchar(100)	'$.tipo_Cuenta_Banco',
						nombre_Cuenta				varchar(100)	'$.nombre_Cuenta',
						tipo_Documento_Causacion	varchar(20)		'$.tipo_Documento_Causacion',
						Numero_Documento_Causacion	varchar(20)		'$.numero_Documento_Causacion',
						tipo_Documento_Alterno		varchar(20)		'$.tipo_Documento_Alterno',
						numero_Documento_Alterno	varchar(20)		'$.numero_Documento_Alterno',
						observaciones				varchar(250)	'$.observaciones',
						base_Retencion				decimal(18,2)	'$.base_Retencion',
						debito						decimal(18,2)	'$.debito',
						credito						decimal(18,2)	'$.credito'
					)

					-- Obtenemos facturas pagadas
					EXEC @resp = api.SpConsumoApiSQL
					@Url = @PathGetRetFactPagas,
					@metodo = 'GET',
					@hasBody = 0,
					@isAuth = 0,
					@authIsRequired = 0,
					--@authorization = @authHeader,
					@contentType = 'application/json',
					@nombreSp = @storedProcedureName,
					@statusText = @statusText OUTPUT,
					@status = @status OUTPUT,
					@authHeader = @authHeader OUTPUT,
					@apiResponse = @ResponseText OUTPUT,
					@ReturnSpErrorMessage = @RecibirErrorMessage OUTPUT						

					IF @resp = 0 BEGIN 
						IF(@ResponseText IS NOT NULL) BEGIN
							IF @status = 200 BEGIN 
								INSERT INTO [log].[LogControlApi]
								SELECT 		 
									CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
									, @storedProcedureName [Lca_NombreApi]
									, @PathGetRetFactPagas [Lca_UrlApi]
									, @statusText [Lca_TipoError]
									, 'OPENJSON' [Lca_PackageName]
									, 1 [Lca_Status]
									, @ResponseText AS [Lca_MensajeEjecucion]
									, GETDATE() [FechaRegistro]

								-- Llenamos tabla #causaciones
								INSERT INTO #causaciones
								SELECT 
									'C' Tipo_Movimiento,
									* 
								FROM OPENJSON(@ResponseText,'$.data')
								WITH (
									empresa						VARCHAR(50)		'$.empresa',
									nombreEmpresa				VARCHAR(150)	'$.nombreEmpresa',
									anio						INT				'$.anio',
									periodo						INT				'$.periodo',
									fecha						VARCHAR(30)		'$.fecha',
									codigo_Tercero				VARCHAR(50)		'$.codigo_Tercero',
									nombre_Tercero				VARCHAR(150)	'$.nombre_Tercero',
									cuenta						VARCHAR(50)		'$.cuenta',
									nombre_Cuenta_Con			VARCHAR(150)	'$.nombre_Cuenta_Con',
									cuenta_Consignacion			VARCHAR(100)	'$.cuenta_Consignacion',
									nombre_Banco				VARCHAR(100)	'$.nombre_Banco',
									tipo_Cuenta_Banco			VARCHAR(100)	'$.tipo_Cuenta_Banco',
									nombre_Cuenta				VARCHAR(100)	'$.nombre_Cuenta',
									tipo_Documento_Causacion	VARCHAR(20)		'$.tipo_Documento_Causacion',
									numero_Documento_Causacion	VARCHAR(20)		'$.numero_Documento_Causacion',
									tipo_Documento_Alterno		VARCHAR(20)		'$.tipo_Documento_Alterno',
									numero_Documento_Alterno	VARCHAR(20)		'$.numero_Documento_Alterno',
									observaciones				VARCHAR(250)	'$.observaciones',
									base_Retencion				DECIMAL(18,2)	'$.base_Retencion',
									debito						DECIMAL(18,2)	'$.debito',
									credito						DECIMAL(18,2)	'$.credito'
								)

								INSERT INTO #controlCausaciones (Tipo_Documento_Causacion,Numero_Documento_Causacion,Tipo_Documento_Alterno, Numero_Documento_Alterno, Anio, Periodo)
								SELECT DISTINCT
									b.Tipo_Documento_Causacion,
									b.Numero_Documento_Causacion,
									b.Tipo_Documento_Alterno,
									b.Numero_Documento_Alterno,
									b.Anio,
									b.Periodo
								FROM #pagos a
									INNER JOIN (
										SELECT 
											*, 
											ROW_NUMBER() OVER(PARTITION BY Anio, Periodo, Tipo_Documento_Alterno, Numero_Documento_Alterno ORDER BY Anio, Periodo, Tipo_Documento_Alterno, Numero_Documento_Alterno) AS RowNumer
										FROM #causaciones
									) AS b ON a.Tipo_Documento_Alterno = b.Tipo_Documento_Alterno AND a.Numero_Documento_Alterno = b.Numero_Documento_Alterno AND b.RowNumer = 1
								WHERE a.Tipo_Documento_Causacion = @TipoDocCausacion AND a.Numero_Documento_Causacion = @NumDocCausacion

								--select * from #controlCausaciones
								--SELECT * FROM #pagos
								--SELECT * FROM #causaciones

								SET @cntmax = (SELECT COUNT(*) FROM #pagos)
								SET @cnt = 1
    
								WHILE @cnt <= @cntmax BEGIN
									SET @TC  = NULL
									SET @NC  = NULL
									SET @TDA = NULL
									SET @NDA = NULL
									SET @TDC = NULL
									SET @NDC = NULL
                    
									--Capturamos documentos de pago
									SELECT
										@TC  = Tipo_Documento_Causacion,
										@NC  = Numero_Documento_Causacion,
										@TDA = Tipo_Documento_Alterno,
										@NDA = Numero_Documento_Alterno
									FROM #pagos
									WHERE id = @cnt	  
    
									--PRINT CONCAT('ALTERNO: ', @TDA, ' - ', @NDA)
    
									--Buscamos documentos de causación
									SELECT
										@TDC = Tipo_Documento_Causacion,
										@NDC = Numero_Documento_Causacion
									FROM #causaciones
									WHERE Tipo_Documento_Alterno = @TDA 
									AND Numero_Documento_Alterno = @NDA
    
									--PRINT CONCAT('CAUSACION: ', @TDC, ' - ', @NDC)

									-- Buscamos el periodo correspondiente de la factura
									SELECT 
										@AnioCausacion = Anio,
										@PeriodoCausacion = Periodo
									FROM #controlCausaciones
									WHERE Tipo_Documento_Causacion = @TDC
									AND Numero_Documento_Causacion = @NDC
									AND Tipo_Documento_Alterno = @TDA
									AND Numero_Documento_Alterno = @NDA
    
									--Registramos retenciones
									INSERT INTO #retenciones (	
										Tipo_Movimiento,			Empresa,					NombreEmpresa,				Anio,
										Periodo,					Fecha,						Codigo_Tercero,				Nombre_Tercero,
										Cuenta,						Nombre_Cuenta_Con,			Cuenta_Consignacion,		Nombre_Banco,
										Tipo_Cuenta_Banco,			Nombre_Cuenta,				Tipo_Documento_Causacion,	Numero_Documento_Causacion,		
										Tipo_Documento_Alterno,		Numero_Documento_Alterno,	Observaciones,				Base_Retencion,			
										Debito,						Credito,					Tdc,						Ndc
									)
									SELECT 
										'R' Tipo_Movimiento,					a.Empresa,								a.NombreEmpresa,				a.Anio,
										a.Periodo,								a.Fecha,								a.Codigo_Tercero,				a.Nombre_Tercero,
										a.Cuenta,								a.Nombre_Cuenta_Con,					a.Cuenta_Consignacion,			a.Nombre_Banco,	
										a.Tipo_Cuenta_Banco,					CONCAT(@TDA,' ',@NDA) Nombre_Cuenta,	a.Tipo_Documento_Causacion,		a.Numero_Documento_Causacion,			
										@TDA Tipo_Documento_Alterno,			@NDA Numero_Documento_Alterno,			a.Observaciones,				(a.Base_Retencion),			
										(a.Debito),								(a.Credito),							@TC,							@NC
									FROM #causaciones a
										LEFT JOIN #retenciones b ON b.Tipo_Documento_Causacion = a.Tipo_Documento_Causacion AND b.Numero_Documento_Causacion = a.Numero_Documento_Causacion
									WHERE a.Tipo_Documento_Causacion = @TDC
										AND a.Numero_Documento_Causacion = @NDC
										AND a.Anio = @AnioCausacion
										AND a.Periodo = @PeriodoCausacion
										AND a.Base_Retencion > 0
										AND b.Tipo_Documento_Causacion IS NULL
										AND b.Numero_Documento_Causacion IS NULL

									SET @cnt = @cnt + 1;
									--BREAK;
								END
    
								SELECT 
									* 
								FROM (
									SELECT 
										Id,														Tipo_Movimiento,						Empresa,								NombreEmpresa,				
										Anio,													Periodo,								Fecha,									Codigo_Tercero,				
										Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
										Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
										Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
										CAST(FORMAT(Base_Retencion,'C','En-Us') AS VARCHAR(100)) Base_Retencion,	
										CAST(FORMAT(Debito,'C','En-Us') AS VARCHAR(100)) Debito,	
										CAST(FORMAT(Credito,'C','En-Us') AS VARCHAR(100)) Credito,										NULL Tdc,								NULL Ndc,
										CONVERT(VARCHAR(100),
											FORMAT(
												SUM(Debito) OVER(PARTITION BY Tipo_Documento_Causacion, Numero_Documento_Causacion ORDER BY Tipo_Documento_Causacion, Numero_Documento_Causacion DESC),
											'C','En-Us'
											)
										) AS Total_Movimiento
									FROM #pagos
									WHERE Tipo_Documento_Causacion = @TipoDocCausacion AND Numero_Documento_Causacion = @NumDocCausacion
									UNION ALL
                                    SELECT 
                                        Id,                                                     Tipo_Movimiento,						Empresa,								NombreEmpresa,				
                                        Anio,													Periodo,								Fecha,									Codigo_Tercero,				
                                        Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
                                        Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
                                        Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
                                        CAST(FORMAT(Base_Retencion,'C','En-Us') AS VARCHAR(100)) Base_Retencion,	                     
                                        CAST(FORMAT(Debito,'C','En-Us') AS VARCHAR(100)) Debito,
                                        CAST(FORMAT(Credito,'C','En-Us') AS VARCHAR(100)) Credito,                                      Tdc,									Ndc,
                                        CONVERT(VARCHAR(100),
                                            FORMAT(
                                                SUM(Credito) OVER(PARTITION BY Tipo_Documento_Causacion, Numero_Documento_Causacion ORDER BY Tipo_Documento_Causacion, Numero_Documento_Causacion DESC),
                                                'C','En-Us'
                                            )
                                        ) AS Total_Movimiento
                                    FROM (
                                        SELECT 
                                            NULL Id,												Tipo_Movimiento,						Empresa,								NombreEmpresa,				
                                            Anio,													Periodo,								Fecha,									Codigo_Tercero,				
                                            Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
                                            Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
                                            Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,				
                                            SUM(Base_Retencion) Base_Retencion,	                    SUM(Debito) Debito,	                    SUM(Credito) Credito,					Tdc,									
                                            Ndc                                            
                                        FROM #retenciones
                                        WHERE Tdc = @TipoDocCausacion AND Ndc = @NumDocCausacion
                                        GROUP BY                                                Tipo_Movimiento,						Empresa,								NombreEmpresa,				
										Anio,													Periodo,								Fecha,									Codigo_Tercero,				
										Nombre_Tercero,											Cuenta,									Nombre_Cuenta_Con,						Cuenta_Consignacion,		
										Nombre_Banco,											Tipo_Cuenta_Banco,						Nombre_Cuenta,							Tipo_Documento_Causacion,	
										Numero_Documento_Causacion,								Tipo_Documento_Alterno,					Numero_Documento_Alterno,				Observaciones,
                                        Tdc,                                                    Ndc
                                    ) AS tbretenciones
								) AS tb1
								ORDER BY Tipo_Movimiento, Id ASC
							END
							ELSE BEGIN 
								--// Dejamos log en [log].[LogControlApi]
								INSERT INTO [log].[LogControlApi]
								SELECT 		 
									CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
									, @storedProcedureName [Lca_NombreApi]
									, @PathGetRetFactPagas [Lca_UrlApi]
									, @statusText [Lca_TipoError]
									, 'OPENJSON' [Lca_PackageName]
									, 0 [Lca_Status]
									, message AS [Lca_MensajeEjecucion]
									, GETDATE() [FechaRegistro]
								FROM OPENJSON(@ResponseText)
								WITH (
									code NVARCHAR(MAX) '$.code',
									message NVARCHAR(MAX) '$.message'
								);
                            
								SET @msg = (SELECT TOP 1 message FROM OPENJSON(@ResponseText) WITH (message NVARCHAR(MAX) '$.message'))
								RAISERROR(@msg,16,-1)
							END
						END
						ELSE BEGIN
							INSERT INTO [log].[LogControlApi]
							SELECT 		 
								CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
								, @storedProcedureName [Lca_NombreApi]
								, @PathGetRetFactPagas [Lca_UrlApi]
								, 'Ok' [Lca_TipoError]
								, 'OPENJSON' [Lca_PackageName]
								, 1 as [Lca_Status]
								, 'No Se encontraron Registros' [Lca_MensajeEjecucion]
								, GETDATE() [FechaRegistro]
						END
					END
					ELSE BEGIN 
						INSERT INTO dbo.LogErrores(Origen,Controlador,Funcion,Descripcion,Usuario,Estado,CreatedAt)
						VALUES('SQL',@storedProcedureName,'2',@RecibirErrorMessage,'',0,GETDATE())

						RAISERROR(@RecibirErrorMessage,16,-1)
					END                    
				END
				ELSE BEGIN 
                    --// Dejamos log en [log].[LogControlApi]
                    INSERT INTO [log].[LogControlApi]
                    SELECT 		 
                        CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
                        , @storedProcedureName [Lca_NombreApi]
                        , @PathGetFactPagas [Lca_UrlApi]
                        , @statusText [Lca_TipoError]
                        , 'OPENJSON' [Lca_PackageName]
                        , 0 [Lca_Status]
                        , message AS [Lca_MensajeEjecucion]
                        , GETDATE() [FechaRegistro]
                    FROM OPENJSON(@ResponseText)
                    WITH (
                        code NVARCHAR(MAX) '$.code',
                        message NVARCHAR(MAX) '$.message'
                    );
                            
                    SET @msg = (SELECT TOP 1 message FROM OPENJSON(@ResponseText) WITH (message NVARCHAR(MAX) '$.message'))
                    RAISERROR(@msg,16,-1)
                END
			END
			ELSE BEGIN
                INSERT INTO [log].[LogControlApi]
                SELECT 		 
                    CONCAT(FORMAT(GETDATE(),'yyyyMMdd'),RIGHT(REPLACE(CONVERT(varchar(255), NEWID()),'-',''),LEN(REPLACE(CONVERT(varchar(255), NEWID()),'-',''))-8)) as [Lca_IdProcesoJob]
                    , @storedProcedureName [Lca_NombreApi]
                    , @PathGetFactPagas [Lca_UrlApi]
                    , 'Ok' [Lca_TipoError]
                    , 'OPENJSON' [Lca_PackageName]
                    , 1 as [Lca_Status]
                    , 'No Se encontraron Registros' [Lca_MensajeEjecucion]
                    , GETDATE() [FechaRegistro]
            END
		END
		ELSE BEGIN 
            INSERT INTO dbo.LogErrores(Origen,Controlador,Funcion,Descripcion,Usuario,Estado,CreatedAt)
            VALUES('SQL',@storedProcedureName,'1',@RecibirErrorMessage,'',0,GETDATE())

            RAISERROR(@RecibirErrorMessage,16,-1)
        END
	END
END TRY
BEGIN CATCH
	INSERT INTO dbo.LogErrores(Origen,Controlador,Funcion,Descripcion,Usuario,Estado,CreatedAt)
    VALUES('SQL',@storedProcedureName,'3',ERROR_MESSAGE(),'',0,GETDATE())

    SELECT CONVERT(bit,0) Status, ERROR_MESSAGE() Message
    PRINT 'ROLLBACK: No se ha realizado ningún cambio'
END CATCH
	
--------------------------------------------------------------------------------------------------
-- fin procedimiento
--------------------------------------------------------------------------------------------------
/*
	PRINT 'Respuesta...'
	PRINT @intOK
	SELECT CONVERT(bit,1) Estado,'Ok' Mensaje
	SET NOCOUNT OFF
*/
