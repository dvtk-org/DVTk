<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
							<xsl:preserve-space elements="Value"/>
	<xsl:param name="filter"/>
	<xsl:param name="filterXMLfilename"/>
	<xsl:variable name="skipped" select="document($filterXMLfilename)/Nodes/skipped"/>
	<xsl:template match="DvtDetailedResultsFile">
		<xsl:param name="ISSEQ"/>
		<xsl:param name="RESULTTYPE"/>
		<xsl:param name="TYPE"/>
		<html>
			<head/>
			<body>
				<h1>DVT Detailed Results File</h1>
				<b>Communication Overview:<br/>
				</b>
				<xsl:for-each select="//MessageUID">
					<xsl:if test="contains(../Name,'RQ')">
						<br/>
					</xsl:if>
					<xsl:if test="contains(../Name,'RSP')">
						<br/>
					</xsl:if>
					<xsl:element name="a">
						<xsl:attribute name="href"><xsl:value-of select="substring-before(//DvtSummaryResultsFilename,'Summary')"/>Detail<xsl:value-of select="substring-before(substring-after(//DvtSummaryResultsFilename,'Summary'),'.xml')"/>.html#UID<xsl:value-of select="."/>
          </xsl:attribute>
            <xsl:text> </xsl:text>
            <br/>
						<xsl:value-of select="../Name"/>
						<xsl:if test="../../ValidationAssociateAc">
							<br/>Association Accept</xsl:if>
						<xsl:if test="../../ValidationReleaseAc">
							<br/>Association Release Accept</xsl:if>
						<xsl:if test="../../ValidationReleaseRq">
							<br/>Association Release Request</xsl:if>
						<xsl:if test="../../ValidationAssociateRq">
							<br/>Association Request</xsl:if>
						<xsl:if test="../../ValidationAssociateRj">
							<br/>Association Reject</xsl:if>
						<xsl:if test="../../ValidationAbortRq">
							<br/>Abort Request</xsl:if>
					</xsl:element>
				</xsl:for-each>
				<xsl:if test="string-length(SubResultsLink/@Index)>0">
					<xsl:call-template name="SubResults">
						<xsl:with-param name="TYPE">Detail_</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
				<xsl:apply-templates/>
				<xsl:call-template name="Directorysum">
					<xsl:with-param name="RESULTTYPE">Detail_</xsl:with-param>
				</xsl:call-template>
				<xsl:call-template name="ValidationCounters"/>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="DvtSummaryResultsFile">
		<xsl:param name="ISSEQ"/>
		<xsl:param name="RESULTTYPE"/>
		<xsl:param name="TYPE"/>
		<html>
			<head/>
			<body>
				<h1>DVT Summary Results File</h1>
				<xsl:if test="string-length(SubResultsLink/@Index)>0">
					<xsl:call-template name="SubResults">
						<xsl:with-param name="TYPE">Summary_</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
				<xsl:apply-templates/>
				<xsl:call-template name="Directorysum">
					<xsl:with-param name="RESULTTYPE">Summary_</xsl:with-param>
				</xsl:call-template>
				<xsl:call-template name="ValidationCounters"/>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="ValidationReport">
		<xsl:for-each select="message">
			<br/>
			<br/>
			<b>Validation of the HL7 message <xsl:value-of select="details/@structure"/>
			</b>
			<table border="1">
				<tbody>
					<tr valign="top">
						<th>Segment</th>
						<th>Field</th>
						<th>Component</th>
						<th>Error Type</th>
						<th>Severity</th>
						<th>Description</th>
						<Segment maxCard="1" minCard="1" name="PID" usage="R"/>
						<Field length="27" minCard="1" name="Patient Identifier List" usage="R"/>
						<Component length="3" name="ID" usage="RE"/>
					</tr>
					<xsl:for-each select="error">
						<tr valign="top">
							<td>
								<xsl:value-of select="profileLoc/Segment/@name"/>
							</td>
							<td>
								<xsl:value-of select="profileLoc/Field/@name"/>
							</td>
							<td>
								<xsl:value-of select="profileLoc/Component /@name"/>
							</td>
							<td>
								<xsl:value-of select="@type"/>
							</td>
							<td>
								<xsl:value-of select="@severity"/>
							</td>
							<td>
								<xsl:for-each select="failureDetails">
									<font color="red">Error: <xsl:value-of select="schema"/>
									</font>
								</xsl:for-each>
							</td>
						</tr>
					</xsl:for-each>
				</tbody>
			</table>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="SubResults">
		<xsl:param name="TYPE"/>
		<table border="1" width="100%" cellpadding="3">
			<font color="#000080">
				<tr>
					<td align="center" valign="top" class="item" colspan="5">
						<b>Emulator Results</b>
					</td>
				</tr>
				<tr>
					<td valign="top" width="10" class="item">Index</td>
					<td valign="top" class="item">Comments</td>
				</tr>
				<xsl:for-each select="SubResultsLink">
					<tr>
						<td valign="top" width="10" class="item">
							<xsl:for-each select="@Index">
								<xsl:element name="a">
									<xsl:attribute name="href"><xsl:value-of select="$TYPE"/><xsl:if test="string-length(../../SessionDetails/SessionId[.])=1">00<xsl:value-of select="../../SessionDetails/SessionId[.]"/></xsl:if><xsl:if test="string-length(../../SessionDetails/SessionId[.])=2">0<xsl:value-of select="../../SessionDetails/SessionId[.]"/></xsl:if><xsl:if test="string-length(../../SessionDetails/SessionId[.])=3"><xsl:value-of select="../../SessionDetails/SessionId[.]"/></xsl:if><xsl:if test="../../SessionDetails/ScpEmulatorType[.]='Printing'">_pr_scp</xsl:if><xsl:if test="../../SessionDetails/ScpEmulatorType[.]='Storage'">_st_scp</xsl:if><xsl:if test="../../SessionDetails/ScpEmulatorType[.]='StorageCommit'">_st_scp</xsl:if>_em_res<xsl:value-of select="."/>.xml</xsl:attribute>
									<xsl:value-of select="."/>
								</xsl:element>
							</xsl:for-each>
						</td>
						<td valign="top" class="item">
							<xsl:if test="(@NrOfErrors[.>'0'])">
								<font color="#FF0000">Error: Record of related image contains an Error.<br/>
								</font>
							</xsl:if>	
							Number of Err: <xsl:for-each select="@NrOfErrors">
								<xsl:value-of select="."/>
							</xsl:for-each>
							<br/>
							Number of Wrn: <xsl:for-each select="@NrOfWarnings">
								<xsl:value-of select="."/>
							</xsl:for-each>
						</td>
					</tr>
				</xsl:for-each>
			</font>
		</table>
	</xsl:template>
	<xsl:template match="SessionDetails">
		<h2>Session Details</h2>
        Session ID: <xsl:value-of select="SessionId"/>
		<br/>
        Session Title: <xsl:value-of select="SessionTitle"/>
		<br/>
        Application Entity Name: <xsl:value-of select="ApplicationEntityName"/>
		<br/>
        Application Entity Version: <xsl:value-of select="ApplicationEntityVersion"/>
		<br/>
        Tester: <xsl:value-of select="Tester"/>
		<br/>
        Test Date: <xsl:value-of select="TestDate"/>
		<br/>
	</xsl:template>
	<xsl:template match="ByteDump">
		<br/>
		<SMALL>
			<i>
				<xsl:value-of select="Description"/> - <xsl:value-of select="ByteStream"/>
			</i>
		</SMALL>
	</xsl:template>
	<xsl:template name="FINDValidationMessages">
		<xsl:for-each select="Message">
			<xsl:call-template name="ValidationMessages"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="Message" name="ValidationMessages">
		<br/>
		<xsl:call-template name="printmessage">
			<xsl:with-param name="type">
				<xsl:value-of select="Type"/>
			</xsl:with-param>
			<xsl:with-param name="index">
				<xsl:value-of select="@Index"/>
			</xsl:with-param>
			<xsl:with-param name="message">
				<xsl:value-of select="Meaning"/>
			</xsl:with-param>
			<xsl:with-param name="messagetype">ValidationMessages</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="VALMessages">
		<xsl:for-each select="Message">
			<tr>
				<td valign="top" width="120"/>
				<td valign="top" width="10"/>
				<td valign="top" width="10"/>
				<td valign="top" width="10"/>
				<td valign="top" width="10"/>
				<td valign="top" width="10"/>
				<td valign="top" width="140"/>
				<td align="center" valign="top" class="item">
					<b>
						<xsl:call-template name="printmessage">
							<xsl:with-param name="type">
								<xsl:value-of select="Type"/>
							</xsl:with-param>
							<xsl:with-param name="index">
								<xsl:value-of select="@Index"/>
							</xsl:with-param>
							<xsl:with-param name="message">
								<xsl:value-of select="Meaning"/>
							</xsl:with-param>
							<xsl:with-param name="messagetype">ValidationMessages</xsl:with-param>
							<xsl:with-param name="location">
								<xsl:choose>
									<xsl:when test="../@Name">on attribute <xsl:value-of select="../@Name"/>
										<xsl:if test="../@Group"> (<xsl:value-of select="../@Group"/>,<xsl:value-of select="../@Element"/>)</xsl:if>
									</xsl:when>
									<xsl:when test="../Attribute/@Name">on attribute <xsl:value-of select="../Attribute/@Name"/>
										<xsl:if test="../Attribute/@Group"> (<xsl:value-of select="../Attribute/@Group"/>,<xsl:value-of select="../Attribute/@Element"/>)</xsl:if>>
          </xsl:when>
									<xsl:when test="../../Attribute/@Name">on attribute <xsl:value-of select="../../Attribute/@Name"/>
										<xsl:if test="../../Attribute/@Group"> (<xsl:value-of select="../../Attribute/@Group"/>,<xsl:value-of select="../../Attribute/@Element"/>)</xsl:if>
									</xsl:when>
									<xsl:when test="../../../Attribute/@Name">on attribute <xsl:value-of select="../../../Attribute/@Name"/>
			on attribute <xsl:if test="../../../Attribute/@Group"> (<xsl:value-of select="../../../Attribute/@Group"/>,<xsl:value-of select="../../../Attribute/@Element"/>)</xsl:if>
									</xsl:when>
									<xsl:when test="../../../../Attribute/@Name">on attribute <xsl:value-of select="../../../../Attribute/@Name"/>
										<xsl:if test="../../../../Attribute/@Group"> (<xsl:value-of select="../../../../Attribute/@Group"/>,<xsl:value-of select="../../../../Attribute/@Element"/>)</xsl:if>
									</xsl:when>
									<xsl:when test="../../../../../Attribute/@Name">on attribute <xsl:value-of select="../../../../../Attribute/@Name"/>
										<xsl:if test="../../../../../Attribute/@Group"> (<xsl:value-of select="../../../../../Attribute/@Group"/>,<xsl:value-of select="../../../../../Attribute/@Element"/>)</xsl:if>
									</xsl:when>
								</xsl:choose>
							</xsl:with-param>
						</xsl:call-template>
					</b>
				</td>
			</tr>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="VALMessagesseq">
		<xsl:for-each select="Message">
			<b>
				<xsl:call-template name="printmessage">
					<xsl:with-param name="type">
						<xsl:value-of select="Type"/>
					</xsl:with-param>
					<xsl:with-param name="index">
						<xsl:value-of select="@Index"/>
					</xsl:with-param>
					<xsl:with-param name="message">
						<xsl:value-of select="Meaning"/>
					</xsl:with-param>
					<xsl:with-param name="messagetype">ValidationMessages</xsl:with-param>
					<xsl:with-param name="location">
						<xsl:choose>
							<xsl:when test="../@Name">on attribute <xsl:value-of select="../@Name"/>
								<xsl:if test="../@Group"> (<xsl:value-of select="../@Group"/>,<xsl:value-of select="../@Element"/>)</xsl:if>
							</xsl:when>
							<xsl:when test="../Attribute/@Name">on attribute <xsl:value-of select="../Attribute/@Name"/>
								<xsl:if test="../Attribute/@Group"> (<xsl:value-of select="../Attribute/@Group"/>,<xsl:value-of select="../Attribute/@Element"/>)</xsl:if>>
          </xsl:when>
							<xsl:when test="../../Attribute/@Name">on attribute <xsl:value-of select="../../Attribute/@Name"/>
								<xsl:if test="../../Attribute/@Group"> (<xsl:value-of select="../../Attribute/@Group"/>,<xsl:value-of select="../../Attribute/@Element"/>)</xsl:if>
							</xsl:when>
							<xsl:when test="../../../Attribute/@Name">on attribute <xsl:value-of select="../../../Attribute/@Name"/>
								<xsl:if test="../../../Attribute/@Group"> (<xsl:value-of select="../../../Attribute/@Group"/>,<xsl:value-of select="../../../Attribute/@Element"/>)</xsl:if>
							</xsl:when>
							<xsl:when test="../../../../Attribute/@Name">on attribute <xsl:value-of select="../../../../Attribute/@Name"/>
								<xsl:if test="../../../../Attribute/@Group"> (<xsl:value-of select="../../../../Attribute/@Group"/>,<xsl:value-of select="../../../../Attribute/@Element"/>)</xsl:if>
							</xsl:when>
							<xsl:when test="../../../../../Attribute/@Name">on attribute <xsl:value-of select="../../../../../Attribute/@Name"/>
								<xsl:if test="../../../../../Attribute/@Group"> (<xsl:value-of select="../../../../../Attribute/@Group"/>,<xsl:value-of select="../../../../../Attribute/@Element"/>)</xsl:if>
							</xsl:when>
						</xsl:choose>
					</xsl:with-param>
				</xsl:call-template>
			</b>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="MessageComparisonResults">
		<br/>
		<b>Message Comparison:</b>
		<table border="1">
			<tbody>
				<tr>
					<th width="12%">Attribute Tag</th>
					<th width="18%">Attribute Name</th>
					<th width="35%">
						<xsl:value-of select="Object1"/>
						<br/>
						<xsl:value-of select="Message1"/>
					</th>
					<th width="35%">
						<xsl:value-of select="Object2"/>
						<br/>
						<xsl:value-of select="Message2"/>
					</th>
				</tr>
				<xsl:for-each select="AttributeComparisonResults">
					<xsl:for-each select="AttributeComparison">
						<tr>
							<th width="12%">
								<xsl:value-of select="@Id"/>
							</th>
							<th width="18%">
								<xsl:value-of select="@Name"/>
							</th>
							<th width="35%">
								<xsl:if test="Value1=''">
									<i>No Value</i>
								</xsl:if>
								<xsl:value-of select="Value1"/>
							</th>
							<th width="35%">
								<xsl:if test="Value2=''">
									<i>No Value</i>
								</xsl:if>
								<xsl:value-of select="Value2"/>
							</th>
						</tr>
					</xsl:for-each>
					<xsl:for-each select="Message">
						<tr>
							<th colspan="4">
								<xsl:call-template name="printmessage">
									<xsl:with-param name="type">
										<xsl:value-of select="Type"/>
									</xsl:with-param>
									<xsl:with-param name="index">
										<xsl:value-of select="@Index"/>
									</xsl:with-param>
									<xsl:with-param name="message">
										<xsl:value-of select="Meaning"/>
									</xsl:with-param>
									<xsl:with-param name="messagetype">ValidationMessages</xsl:with-param>
								</xsl:call-template>
							</th>
						</tr>
					</xsl:for-each>
				</xsl:for-each>
			</tbody>
		</table>
		<br/>
	</xsl:template>
	<xsl:template match="HTMLUserActivity">
		<xsl:call-template name="printmessage">
			<xsl:with-param name="type">
				<xsl:value-of select="@Level"/>
			</xsl:with-param>
			<xsl:with-param name="index">
				<xsl:value-of select="@Index"/>
			</xsl:with-param>
			<xsl:with-param name="message">
				<xsl:value-of select="."/>
			</xsl:with-param>
			<xsl:with-param name="messagetype">HTMLUserActivity</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="UserActivity">
		<xsl:call-template name="printmessage">
			<xsl:with-param name="type">
				<xsl:value-of select="@Level"/>
			</xsl:with-param>
			<xsl:with-param name="index">
				<xsl:value-of select="@Index"/>
			</xsl:with-param>
			<xsl:with-param name="message">
				<xsl:value-of select="."/>
			</xsl:with-param>
			<xsl:with-param name="messagetype">UserActivity</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="Activity">
		<xsl:if test="@Level[.='Error']">
			<xsl:call-template name="printmessage">
				<xsl:with-param name="type">
					<xsl:value-of select="@Level"/>
				</xsl:with-param>
				<xsl:with-param name="index">
					<xsl:value-of select="@Index"/>
				</xsl:with-param>
				<xsl:with-param name="message">
					<xsl:value-of select="."/>
				</xsl:with-param>
				<xsl:with-param name="messagetype">Activity</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="@Level[.='Warning']">
			<xsl:call-template name="printmessage">
				<xsl:with-param name="type">
					<xsl:value-of select="@Level"/>
				</xsl:with-param>
				<xsl:with-param name="index">
					<xsl:value-of select="@Index"/>
				</xsl:with-param>
				<xsl:with-param name="message">
					<xsl:value-of select="."/>
				</xsl:with-param>
				<xsl:with-param name="messagetype">Activity</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<xsl:if test="@Level[.='Scripting']">
			<br/>
			<SMALL>
				<i>
					<xsl:value-of select="."/>
				</i>
			</SMALL>
		</xsl:if>
		<xsl:if test="@Level[.='ScriptName']">
			<br/>
			<b>
						Script Name:<xsl:value-of select="."/>
			</b>
		</xsl:if>
		<xsl:if test="@Level[.='Script']">
			<br/>
			<SMALL>
				<b>
					<xsl:value-of select="."/>
				</b>
			</SMALL>
		</xsl:if>
		<xsl:if test="@Level[.='None']">
			<br/>
			<SMALL>
				<i>
					<xsl:value-of select="."/>
				</i>
			</SMALL>
		</xsl:if>
		<xsl:if test="@Level[.='Info']">
			<br/>
			<i>
				<xsl:value-of select="."/>
			</i>
		</xsl:if>
		<xsl:if test="@Level[.='ConditionText']">
			<br/>
			<i>
				<xsl:value-of select="."/>
			</i>
		</xsl:if>
		<xsl:if test="@Level[.='Debug']">
			<br/>
			<i>
				<xsl:value-of select="."/>
			</i>
		</xsl:if>
		<xsl:if test="@Level[.='DCM']">
			<br/>
			<br/>
			<b>
				<i>
				Media file <xsl:element name="a">
						<xsl:attribute name="href"><xsl:value-of select="."/></xsl:attribute>
						<xsl:value-of select="."/>
					</xsl:element> created, click to open DCM file in viewer.

			<br/>
				</i>
			</b>
		</xsl:if>
		<xsl:if test="@Level[.='DicomObjectRelationship']">
			<br/>
			<i>
				<xsl:value-of select="."/>
			</i>
		</xsl:if>
	</xsl:template>
	<xsl:template match="DvtDetailedResultsFilename">
	</xsl:template>
	<xsl:template match="DvtSummaryResultsFilename">

	</xsl:template>
	<xsl:template match="Send">
		<br/>
		<br/>
		<b>Send: </b>
		<xsl:apply-templates/>
		<br/>
	</xsl:template>
	<xsl:template match="Import">
		<br/>
		<br/>
		<b>Imported: </b>
		<xsl:apply-templates/>
		<br/>
	</xsl:template>
	<xsl:template match="Create">
		<br/>
		<br/>
		<b>Create: </b>
		<xsl:for-each select="CommandField">
			<b>
				<xsl:value-of select="."/> &#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="CommandSetRefId">
			<b>
				<xsl:value-of select="."/> &#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="IodId">
			<b>
				"<xsl:value-of select="."/>"&#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="DataSetRefId">
			<b>
				<xsl:value-of select="."/>
			</b>
		</xsl:for-each>
		<br/>
		<xsl:for-each select="CommandSet">
			<i>        Command Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="DataSet">
			<i>    Data Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<br/>
	</xsl:template>
	<xsl:template match="Set">
		<br/>
		<br/>
		<b>Set: </b>
		<xsl:for-each select="CommandField">
			<b>
				<xsl:value-of select="."/> &#160;
							</b>
		</xsl:for-each>
		<xsl:for-each select="CommandSetRefId">
			<b>
				<xsl:value-of select="."/> &#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="IodId">
			<b>
				"<xsl:value-of select="."/>"&#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="DataSetRefId">
			<b>
				<xsl:value-of select="."/>
			</b>
		</xsl:for-each>
		<br/>
		<xsl:for-each select="CommandSet">
			<i>        Command Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="Dataset">
			<i>    Data Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<br/>
	</xsl:template>
	<xsl:template match="Display">
		<br/>
DISPLAY:			<xsl:call-template name="Attribute">
			<xsl:with-param name="ISSEQ"/>
		</xsl:call-template>
		<xsl:for-each select="CommandField">
			<b>
				<xsl:value-of select="."/> &#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="CommandSetRefId">
			<b>
				<xsl:value-of select="."/> &#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="IodId">
			<b>
				"<xsl:value-of select="."/>"&#160;
			</b>
		</xsl:for-each>
		<xsl:for-each select="DataSetRefId">
			<b>
				<xsl:value-of select="."/>
			</b>
		</xsl:for-each>
		<xsl:for-each select="FileHead">
			<i> Media File <xsl:for-each select="TransferSyntax">
			(TransferSyntax=<xsl:value-of select="."/>)
			<br/>
				</xsl:for-each>
			</i>
			<br/>
		</xsl:for-each>
		<xsl:for-each select="CommandSet">
			<i>        Command Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="FileMetaInformation">
			<i>        File Meta Information attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
			<br/>
		</xsl:for-each>
		<xsl:for-each select="DicomMessage/Command">
			<i>   Command Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="DicomMessage/Dataset">
			<i>    Data Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="Dataset">
			<i>    Data Set attributes</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="Receive">
		<br/>
		<br/>
		<b>Received: </b>
		<xsl:apply-templates/>
		<br/>
	</xsl:template>
	<xsl:template match="DicomMessage">
		<xsl:for-each select="Command">
			<br/>
			<i>
				<b>
					<xsl:value-of select="@Id"/>
				</b>
			</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:for-each select="Dataset">
			<br/>
			<i>
        <xsl:if test ="count(@Name)>4">
          <b>
            <xsl:value-of select="@Name"/>
          </b>
        </xsl:if>
			</i>
			<br/>
			<xsl:call-template name="Attribute">
				<xsl:with-param name="ISSEQ"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="Attribute">
		<xsl:param name="ISSEQ"/>
		<xsl:for-each select="*">
			<xsl:if test="name()='ItemIntroducer'"># (0x<xsl:value-of select="@Group"/>
				<xsl:value-of select="@Element"/>, <xsl:value-of select="@Length"/>) # ItemIntroducer <br/>
			</xsl:if>
			<xsl:if test="name()='ItemDelimiter'"># (0x<xsl:value-of select="@Group"/>
				<xsl:value-of select="@Element"/>, <xsl:value-of select="@Length"/>) # ItemDelimiter <br/>
			</xsl:if>
			<xsl:if test="name()='Attribute'">
				<xsl:value-of select="$ISSEQ"/>(0x<xsl:value-of select="@Group"/>
				<xsl:value-of select="@Element"/>,<xsl:value-of select="@VR"/>
				<xsl:if test="not(@VR='')">,</xsl:if>
        
				<xsl:for-each select="Values">
					<xsl:if test="count(Value)=0">
						<xsl:if test="count(Sequence)=0">""</xsl:if>
					</xsl:if>
					<xsl:for-each select="FileName">
						<xsl:value-of select="."/>
					</xsl:for-each>
					<xsl:for-each select="Value">
						<xsl:if test="not(../../@VR[.='SS'])">
							<xsl:if test="not(../../@VR[.='SL'])">
								<xsl:if test="not(../../@VR[.='US'])">
									<xsl:if test="not(../../@VR[.='UL'])">
										<xsl:if test="not(../../@VR[.='AT'])">
                      <xsl:text>"</xsl:text>
                    </xsl:if>
									</xsl:if>
								</xsl:if>
							</xsl:if>
						</xsl:if>
            <xsl:value-of select="."/>
            <xsl:variable name="next" select="following-sibling::*"/>
            <xsl:if test="$next">
              <xsl:if test="name($next)='Unicode'">
                <xsl:text>)#</xsl:text>
                <xsl:value-of select="parent::*/parent::*/@Name"/>                
                <xsl:text>, display as:"</xsl:text>
                <b><xsl:value-of select="$next"/></b>
              </xsl:if>
            </xsl:if>
            <xsl:if test="not(../../@VR[.='SS'])">
							<xsl:if test="not(../../@VR[.='SL'])">
								<xsl:if test="not(../../@VR[.='US'])">
									<xsl:if test="not(../../@VR[.='UL'])">
										<xsl:if test="not(../../@VR[.='AT'])">
                      <xsl:text>"</xsl:text>
                    </xsl:if>
									</xsl:if>
								</xsl:if>
							</xsl:if>
						</xsl:if>
						<xsl:if test="not(last()=position())">,</xsl:if>
					</xsl:for-each>
					<xsl:for-each select="Sequence">
						<br/>
						<xsl:for-each select="Item">
# Item Number <xsl:value-of select="@Number"/>
							<br/>
							<xsl:if test="not($ISSEQ='')">
								<xsl:call-template name="Attribute">
									<xsl:with-param name="ISSEQ">&#160;&#160;<xsl:value-of select="$ISSEQ"/>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:if>
							<xsl:if test="$ISSEQ=''">
								<xsl:call-template name="Attribute">
									<xsl:with-param name="ISSEQ">
										<xsl:value-of select="$ISSEQ"/>&#160;&#160;></xsl:with-param>
								</xsl:call-template>
							</xsl:if>
							<xsl:if test="not(last()=position())">,<br/>
							</xsl:if>
						</xsl:for-each>
						<xsl:for-each select="SequenceDelimiter"># (0x<xsl:value-of select="@Group"/>
							<xsl:value-of select="@Element"/>, <xsl:value-of select="@Length"/>) # SequenceDelimiter<br/>
						</xsl:for-each>
					</xsl:for-each>
          <xsl:if test="not(Unicode)">
            <xsl:text>) # </xsl:text>
            <xsl:value-of select="parent::*/@Name"/>
          </xsl:if>
				</xsl:for-each>        

        <xsl:for-each select="Values/Sequence"> with length of: <xsl:value-of select="@Length"/>
				</xsl:for-each>
				<br/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="AssociateRq">
		<xsl:call-template name="A_ASSOCIATE_RQ_AC">
			<xsl:with-param name="ASSOCTYPE">A_ASSOCIATE_RQ</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="AssociateAc">
		<xsl:call-template name="A_ASSOCIATE_RQ_AC">
			<xsl:with-param name="ASSOCTYPE">A_ASSOCIATE_AC</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="AssociateRj">
		<xsl:call-template name="A_ASSOCIATE_RJ"/>
	</xsl:template>
	<xsl:template match="AbortRq">
		<xsl:call-template name="A_ABORT"/>
	</xsl:template>
	<xsl:template match="ReleaseRq">
		<xsl:call-template name="A_RELEASE_RQ"/>
	</xsl:template>
	<xsl:template match="ReleaseRp">
		<xsl:call-template name="A_RELEASE_RP"/>
	</xsl:template>
	<xsl:template name="A_ASSOCIATE_RQ_AC">
		<xsl:param name="ASSOCTYPE"/>
		<b>
			<xsl:value-of select="$ASSOCTYPE"/> Message
    </b>
		<br/>	Protocol Version: <xsl:for-each select="ProtocolVersion">
			<xsl:value-of select="."/>
		</xsl:for-each>
		<br/>	Called AE Title: <xsl:for-each select="CalledAETitle">
			<xsl:value-of select="."/>
		</xsl:for-each>
		<br/>	Calling AE Title: <xsl:for-each select="CallingAETitle">
			<xsl:value-of select="."/>
		</xsl:for-each>
		<br/>	Application Context: <xsl:for-each select="ApplicationContextName">
			<xsl:value-of select="."/>
		</xsl:for-each>
		<br/> Presentation Context Item(s):  
				<xsl:for-each select="PresentationContexts">
			<xsl:for-each select="PresentationContext">
				<br/>
				<i>
						Presentation Context ID: &#160;&#160; ID: <xsl:value-of select="@Id"/>
				</i>
				<xsl:if test="count(@Result)>0">
					<br/>	&#160;&#160;&#160;&#160;> Result: <xsl:value-of select="@Result"/>
				</xsl:if>
				<br/>	&#160;&#160;&#160;&#160; > Abstract Syntax: <xsl:value-of select="@AbstractSyntaxName"/>
				<xsl:for-each select="TransferSyntaxes">
					<xsl:for-each select="TransferSyntax">
						<br/>	&#160;&#160;&#160;&#160; > Transfer Syntax: 
					<xsl:value-of select="."/>
					</xsl:for-each>
				</xsl:for-each>
				<xsl:for-each select="TransferSyntax">
					<br/>	&#160;&#160;&#160;&#160; > Transfer Syntax: 
					<xsl:value-of select="."/>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:for-each>
		<br/> User Information:  <xsl:for-each select="UserInformation">
			<xsl:if test="count(MaximumLengthReceived)>0">
				<br/>	&#160;&#160;&#160;&#160;> Maximum Length: <xsl:value-of select="MaximumLengthReceived"/>
			</xsl:if>
			<xsl:if test="count(ImplementationClassUid)>0">
				<br/>	&#160;&#160;&#160;&#160;> Implementation Class UID: <xsl:value-of select="ImplementationClassUid"/>
			</xsl:if>
			<xsl:if test="count(ImplementationVersionName)>0">
				<br/>	&#160;&#160;&#160;&#160;> Implementation Version Name: <xsl:value-of select="ImplementationVersionName"/>
			</xsl:if>
			<xsl:if test="count(AsynchronousOperationsWindow)>0">
				<br/>	Asynchronous Operations Window: 
				<xsl:for-each select="AsynchronousOperationsWindow">
					<br/> &#160;&#160;&#160;&#160;> Maximum Number Operations Invoked: <xsl:value-of select="@Invoked"/>
					<br/> &#160;&#160;&#160;&#160;> Maximum Number Operations Performed: <xsl:value-of select="@Peformed"/>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="count(ScpScuRoleSelections)>0">
				<xsl:for-each select="ScpScuRoleSelections">
					<br/>	SCP SCU Role Selection(s): <xsl:for-each select="ScpScuRoleSelection">
						<br/>	 SOP Class UID: 	<xsl:value-of select="@Uid"/>
						<br/> &#160;&#160;&#160;&#160;> SCU Role: 	<xsl:value-of select="@ScuRole"/>
						<br/> &#160;&#160;&#160;&#160;> SCP Role: 	<xsl:value-of select="@ScpRole"/>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="count(SopClassExtendedNegotiations)>0">
				<xsl:for-each select="SopClassExtendedNegotiations">
					<br/>	SOP Class Extended Negotiation(s): <xsl:for-each select="SopClassExtendedNegotiation">
						<br/>	 SOP Class UID: 	<xsl:value-of select="@Uid"/>
						<br/> &#160;&#160;&#160;&#160;>  Service Class Application Information: 	<xsl:value-of select="@AppInfo"/>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:if>
			<br/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="A_ASSOCIATE_RJ">
		<b>A-ASSOCIATE-RJ Message</b>
		<br/>	Result: <xsl:for-each select="Result">
			<xsl:value-of select="."/>
			<xsl:if test="(.)='1'"> (Rejected-permanent)			</xsl:if>
			<xsl:if test="(.)='2'"> (Rejected-transient)			</xsl:if>
		</xsl:for-each>
		<br/>	Source: <xsl:for-each select="Source">
			<xsl:value-of select="."/>
			<xsl:if test="(.)='1'"> (DICOM UL service-user)			</xsl:if>
			<xsl:if test="(.)='2'"> (DICOM UL service-provider (ACSE related function))			</xsl:if>
			<xsl:if test="(.)='3'"> (DICOM UL service-provider (Presentation related function))			</xsl:if>
		</xsl:for-each>
		<br/>	Reason: <xsl:for-each select="Reason">
			<xsl:value-of select="."/>
			<xsl:if test="(../Source)='1'">
				<xsl:if test="(.)='1'"> (no-reason-given)			</xsl:if>
				<xsl:if test="(.)='2'"> (application-Context-name-not-supported)			</xsl:if>
				<xsl:if test="(.)='3'"> (calling-AE-title-not-recognized)			</xsl:if>
				<xsl:if test="(.)='4'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='5'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='6'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='7'"> (called-AE-title-not-recognized)			</xsl:if>
				<xsl:if test="(.)='8'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='9'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='10'"> (reserved)			</xsl:if>
			</xsl:if>
			<xsl:if test="(../Source)='2'">
				<xsl:if test="(.)='1'"> (no-reason-given)			</xsl:if>
				<xsl:if test="(.)='2'"> (protocol-version-not-supported)			</xsl:if>
			</xsl:if>
			<xsl:if test="(../Source)='3'">
				<xsl:if test="(.)='1'"> (temporary-congestion)			</xsl:if>
				<xsl:if test="(.)='2'"> (local-limit-exceeded)			</xsl:if>
				<xsl:if test="(.)='3'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='4'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='5'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='6'"> (reserved)			</xsl:if>
				<xsl:if test="(.)='7'"> (reserved)			</xsl:if>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="A_ABORT">
		<b>A-ABORT Message</b>
		<br/>	Reason: <xsl:for-each select="Reason">
			<xsl:value-of select="."/>
			<xsl:if test="(.)='0'"> (Reason-not-specified)			</xsl:if>
			<xsl:if test="(.)='1'"> (Unrecognized-PDU)			</xsl:if>
			<xsl:if test="(.)='2'"> (Unexpected-PDU)			</xsl:if>
			<xsl:if test="(.)='3'"> (Reserved)			</xsl:if>
			<xsl:if test="(.)='4'"> (Unrecognized-PDU parameter)	</xsl:if>
			<xsl:if test="(.)='5'"> (Unexpected-PDU parameter)		</xsl:if>
			<xsl:if test="(.)='6'"> (Invalid-PDU-parameter value)			</xsl:if>
		</xsl:for-each>
		<br/>	Source: <xsl:for-each select="Source">
			<xsl:value-of select="."/>
			<xsl:if test="(.)='0'"> (DICOM UL service-user (initiated abort))			</xsl:if>
			<xsl:if test="(.)='1'"> (Reserved)			</xsl:if>
			<xsl:if test="(.)='2'"> (DICOM UL service-provider (initiated abort))			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="A_RELEASE_RQ">
		<b>A-RELEASE-RQ Message</b>
	</xsl:template>
	<xsl:template name="A_RELEASE_RP">
		<b>A-RELEASE-RP Message</b>
	</xsl:template>
	<xsl:template match="ValidationAssociateAc">
		<br/><xsl:element name="a"><xsl:attribute name="NAME">UID<xsl:value-of select="MessageUID"/></xsl:attribute></xsl:element>
		<font color="#000080">
			<b>Validate: </b>
			<xsl:call-template name="Val_A_ASSOCIATE_RQ_AC">
				<xsl:with-param name="ASSOCTYPE">A_ASSOCIATE_AC</xsl:with-param>
			</xsl:call-template>
		</font>
		<br/>
	</xsl:template>
	<xsl:template match="ValidationAssociateRq">
		<br/><xsl:element name="a"><xsl:attribute name="NAME">UID<xsl:value-of select="MessageUID"/></xsl:attribute></xsl:element>
		<font color="#000080">
			<b>Validate: </b>
			<xsl:call-template name="Val_A_ASSOCIATE_RQ_AC">
				<xsl:with-param name="ASSOCTYPE">A_ASSOCIATE_RQ</xsl:with-param>
			</xsl:call-template>
		</font>
		<br/>
	</xsl:template>
	<xsl:template match="ValidationDicomMessage" name="objectresults">
		<br/><xsl:element name="a"><xsl:attribute name="NAME">UID<xsl:value-of select="MessageUID"/></xsl:attribute></xsl:element>
		<font color="#000080">
			<b>
				<xsl:value-of select="Name"/>
			</b>
			<br/>
			<xsl:for-each select="Module">
				<br/>
				<table border="1" width="100%" cellpadding="3">
					<font color="#000080">
						<tr>
							<td align="center" valign="top" class="item" colspan="8">
								<b>
					Module: <xsl:value-of select="@Name"/> (<xsl:value-of select="@Usage"/>)
									</b>
							</td>
						</tr>
						<tr>
							<td valign="top" width="120" class="item">Attribute</td>
							<td valign="top" width="10" class="item">Def VR</td>
							<td valign="top" width="10" class="item">Dcm VR</td>
							<td valign="top" width="10" class="item">Type</td>
							<td valign="top" width="10" class="item">Pr</td>
							<td valign="top" width="10" class="item">Len</td>
							<td valign="top" width="140" class="item">Attribute Name</td>
							<td valign="top" class="item">Value(s) and Comments</td>
						</tr>
						<xsl:call-template name="VALAttributes">
							<xsl:with-param name="VALISSEQ">	</xsl:with-param>
						</xsl:call-template>
					</font>
				</table>
				<xsl:call-template name="FindMessages"/>
			</xsl:for-each>
			<xsl:for-each select="AdditionalAttributes">
				<table border="1" width="100%" cellpadding="3">
					<font color="#000080">
						<tr>
							<td align="center" valign="top" class="item" colspan="8">
								<b>
					Additional Attributes
									</b>
							</td>
						</tr>
						<tr>
							<td valign="top" width="120" class="item">Attribute</td>
							<td valign="top" width="10" class="item">Def VR</td>
							<td valign="top" width="10" class="item">Dcm VR</td>
							<td valign="top" width="10" class="item">Type</td>
							<td valign="top" width="10" class="item">Pr</td>
							<td valign="top" width="10" class="item">Len</td>
							<td valign="top" width="140" class="item">Attribute Name</td>
							<td valign="top" class="item">Value(s) and Comments</td>
						</tr>
						<xsl:call-template name="VALAttributes">
							<xsl:with-param name="VALISSEQ">	</xsl:with-param>
						</xsl:call-template>
					</font>
				</table>
				<xsl:call-template name="FindMessages"/>
			</xsl:for-each>
			<xsl:call-template name="FindMessages"/>
		</font>
	</xsl:template>
	<xsl:template name="Directorysum">
		<xsl:param name="RESULTTYPE"/>
		<xsl:for-each select="//DirectoryRecordLinks">
			<table border="1" width="100%" cellpadding="3">
				<font color="#000080">
					<tr>
						<td align="center" valign="top" class="item" colspan="5">
							<b>
					Directory Record TOC
									</b>
						</td>
					</tr>
					<tr>
						<td valign="top" width="120" class="item">Record Type</td>
						<td valign="top" width="10" class="item">Index</td>
						<td valign="top" width="10" class="item">Record Offset</td>
						<td valign="top" width="10" class="item">Reference Count</td>
						<td valign="top" class="item">Comments</td>
					</tr>
					<xsl:for-each select="DirectoryRecordLink ">
						<tr>
							<td valign="top" width="120" class="item">
								<xsl:for-each select="@Type">
									<xsl:value-of select="."/>
								</xsl:for-each>
							</td>
							<td valign="top" width="10" class="item">
								<xsl:for-each select="@Index">
									<xsl:element name="a">
										<xsl:attribute name="href"><xsl:copy-of select="$RESULTTYPE"/><xsl:if test="string-length(../../../../../SessionDetails/SessionId[.])=1">00<xsl:value-of select="../../../../../SessionDetails/SessionId[.]"/></xsl:if><xsl:if test="string-length(../../../../../SessionDetails/SessionId[.])=2">0<xsl:value-of select="../../../../../SessionDetails/SessionId[.]"/></xsl:if><xsl:if test="string-length(../../../../../SessionDetails/SessionId[.])=3"><xsl:value-of select="../../../../../SessionDetails/SessionId[.]"/></xsl:if>_<xsl:value-of select="../../../DICOMDIRName"/>_res<xsl:value-of select="."/>.xml</xsl:attribute>
										<xsl:value-of select="."/>
									</xsl:element>
								</xsl:for-each>
							</td>
							<td valign="top" width="10" class="item">
								<xsl:for-each select="@RecordOffset">
									<xsl:value-of select="."/>
								</xsl:for-each>
							</td>
							<td valign="top" width="10" class="item">
								<xsl:for-each select="@ReferenceCount">
									<xsl:value-of select="."/>
								</xsl:for-each>
							</td>
							<td valign="top" class="item">
								<xsl:if test="(@NrOfErrors[.>'0'])">
									<font color="#FF0000">Error: Record of related image contains an Error.<br/>
									</font>
								</xsl:if>	
							Number of Err: <xsl:for-each select="@NrOfErrors">
									<xsl:value-of select="."/>
								</xsl:for-each>
								<br/>
							Number of Wrn: <xsl:for-each select="@NrOfWarnings">
									<xsl:value-of select="."/>
								</xsl:for-each>
															
								
								 &#160;
							<xsl:for-each select="Message">
									<xsl:call-template name="printmessage">
										<xsl:with-param name="type">
											<xsl:value-of select="Type"/>
										</xsl:with-param>
										<xsl:with-param name="index">
											<xsl:value-of select="@Index"/>
										</xsl:with-param>
										<xsl:with-param name="message">
											<xsl:value-of select="Meaning"/>
										</xsl:with-param>
										<xsl:with-param name="messagetype">ValidationMessages</xsl:with-param>
									</xsl:call-template>
								</xsl:for-each>
							</td>
						</tr>
					</xsl:for-each>
				</font>
			</table>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="VALAttributes">
		<xsl:param name="VALISSEQ"/>
		<xsl:param name="seqattr"/>
		<xsl:for-each select="Attribute">
			<tr>
				<td valign="top" width="120">
					<xsl:value-of select="$VALISSEQ"/>(<xsl:value-of select="@Group"/>,<xsl:value-of select="@Element"/>)</td>
				<td valign="top" width="10">
<xsl:value-of select="@VR"/>
					<xsl:value-of select="@DefVR"/>
				</td>
				<td valign="top" width="10">
					<xsl:value-of select="@DcmVR"/>
				</td>
				<td valign="top" width="10">
					<xsl:value-of select="@Type"/>
				</td>
				<td valign="top" width="10">
					<xsl:value-of select="@Present"/>
				</td>
				<td valign="top" width="10">
					<xsl:value-of select="@Length"/>
				</td>
				<td valign="top" width="140">
					<xsl:value-of select="@Name"/>
				</td>
				<td valign="top">
					<xsl:for-each select="Values">
						<xsl:if test="Sequence">
							<xsl:call-template name="VALMessagesseq"/>
						</xsl:if>
						<xsl:for-each select="Value">
							<pre><xsl:value-of select="."/></pre>
              <xsl:variable name="next" select="following-sibling::*"/>
              <xsl:if test="$next">
                <xsl:if test="name($next)='Unicode'">
                  <br></br>
                  <xsl:text>Display as: </xsl:text>
                  <b><xsl:value-of select="$next"/></b>
                </xsl:if>
              </xsl:if>
							<xsl:if test="not(last()=position())">
								<br/>
							</xsl:if>
						</xsl:for-each>
						<xsl:for-each select="Value">
							<xsl:call-template name="VALMessages"/>
						</xsl:for-each>
						<xsl:for-each select="Sequence">
							<xsl:call-template name="VALMessages"/>
							<xsl:for-each select="Item">
								<xsl:call-template name="VALMessages"/>
								<xsl:if test="not($VALISSEQ='')">
									<xsl:call-template name="VALAttributes">
										<xsl:with-param name="VALISSEQ">&#160;&#160;<xsl:value-of select="$VALISSEQ"/>></xsl:with-param>
									</xsl:call-template>
								</xsl:if>
								<xsl:if test="$VALISSEQ=''">
									<xsl:call-template name="VALAttributes">
										<xsl:with-param name="VALISSEQ">
											<xsl:value-of select="$VALISSEQ"/>&#160;&#160;></xsl:with-param>
									</xsl:call-template>
								</xsl:if>
								<xsl:if test="not(last()=position())">
									<tr>
										<td>
											<i>next Item</i>
										</td>
									</tr>
								</xsl:if>
							</xsl:for-each>
						</xsl:for-each>
						<xsl:call-template name="VALMessages"/>
					</xsl:for-each>
				</td>
			</tr>
			<xsl:call-template name="VALMessages"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="Val_A_ASSOCIATE_RQ_AC">
		<xsl:param name="ASSOCTYPE"/>
		<b>
			<xsl:value-of select="$ASSOCTYPE"/> Message
    </b>
		<xsl:if test="count(ProtocolVersion/@Value)>0">
			<br/>	Protocol Version: <xsl:for-each select="ProtocolVersion">
				<xsl:value-of select="@Value"/>
				<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="count(CalledAETitle/@Value)>0">
			<br/>	Called AE Title: <xsl:for-each select="CalledAETitle">
				<xsl:value-of select="@Value"/>
				<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="count(CallingAETitle/@Value)>0">
			<br/>	Calling AE Title: <xsl:for-each select="CallingAETitle">
				<xsl:value-of select="@Value"/>
				<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="count(ApplicationContextName/@Value)>0">
			<br/>	Application Context: <xsl:for-each select="ApplicationContextName">
				<xsl:value-of select="@Value"/>
				<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
				<xsl:apply-templates/>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="count(PresentationContexts/PresentationContext/Id/@Value)>0">
			<br/> Presentation Context Item(s): <xsl:for-each select="PresentationContexts">
				<xsl:for-each select="PresentationContext">
					<xsl:for-each select="Id">
						<br/>Presentation Context:&#160;&#160; ID: <xsl:value-of select="@Value"/>
						<xsl:call-template name="FINDValidationMessages"/>
					</xsl:for-each>
					<xsl:if test="count(Result/@Value)>0">
						<xsl:for-each select="Result">
							<br/>	&#160;&#160;&#160;&#160;> Result: <xsl:value-of select="@Value"/>
							<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="count(AbstractSyntaxName/@Value)>0">
						<xsl:for-each select="AbstractSyntaxName">
							<br/>	&#160;&#160;&#160;&#160;> Abstract Syntax: <xsl:value-of select="@Value"/>
							<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="count(TransferSyntaxes/TransferSyntax/@Value)>0">
						<xsl:for-each select="TransferSyntaxes">
							<xsl:for-each select="TransferSyntax">
								<br/>	&#160;&#160;&#160;&#160; > Transfer Syntax: 
					<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="count(TransferSyntax/@Value)>0">
						<xsl:for-each select="TransferSyntax">
							<br/>	&#160;&#160;&#160;&#160; > Transfer Syntax:  
					<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="count(UserInformation)>0">
			<br/> User Information:  <xsl:for-each select="UserInformation">
				<xsl:if test="count(MaximumLengthReceived)>0">
					<br/>&#160;&#160;&#160;&#160;>	Maximum Length: <xsl:for-each select="MaximumLengthReceived">
						<xsl:value-of select="@Value"/>
						<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
						<xsl:call-template name="FINDValidationMessages"/>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count(ImplementationClassUid)>0">
					<br/>&#160;&#160;&#160;&#160;>	Implementation Class UID: <xsl:for-each select="ImplementationClassUid">
						<xsl:value-of select="@Value"/>
						<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
						<xsl:call-template name="FINDValidationMessages"/>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count(ImplementationVersionName)>0">
					<br/>&#160;&#160;&#160;&#160;>	Implementation Version Name: <xsl:for-each select="ImplementationVersionName">
						<xsl:value-of select="@Value"/>
						<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
						<xsl:call-template name="FINDValidationMessages"/>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count(AsynchronousOperationsWindow)>0">
					<br/>&#160;&#160;&#160;&#160;>	Asynchronous Operations Window: <xsl:for-each select="AsynchronousOperationsWindow">
						<xsl:for-each select="Invoked">
							<br/>	 &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>  Maximum Number Operations Invoked: <xsl:value-of select="@Value"/>
							<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
						<xsl:for-each select="Performed">
							<br/>	 &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>  Maximum Number Operations Performed: <xsl:value-of select="@Value"/>
							<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
						<xsl:call-template name="FINDValidationMessages"/>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count(ScpScuRoleSelections)>0">
					<xsl:for-each select="ScpScuRoleSelections">
						<br/>&#160;&#160;&#160;&#160;>	SCP SCU Role Selection(s): <xsl:for-each select="ScpScuRoleSelection">
							<xsl:for-each select="Uid">
								<br/> &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>> SOP Class UID: 	<xsl:value-of select="@Value"/>
								<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:for-each select="ScuRole">
								<br/> &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>> SCU Role: 	<xsl:value-of select="@Value"/>
								<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:for-each select="ScpRole">
								<br/> &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>> SCP Role: 	<xsl:value-of select="@Value"/>
								<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count(SopClassExtendedNegotiations)>0">
					<xsl:for-each select="SopClassExtendedNegotiations">
						<xsl:for-each select="Uid">
							<br/>	&#160;&#160;&#160;&#160;>SOP Class Extended Negotiation(s): <xsl:for-each select="SopClassExtendedNegotiation">
								<br/>	 &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>>  SOP Class UID: 	<xsl:value-of select="@Value"/>
								<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:for-each select="AppInfo">
								<br/> &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;>>>  Service Class Application Information: 	<xsl:value-of select="@Value"/>
								<xsl:if test="count(@Meaning)>0">(<xsl:value-of select="@Meaning"/>)</xsl:if>
								<xsl:call-template name="FINDValidationMessages"/>
							</xsl:for-each>
							<xsl:call-template name="FINDValidationMessages"/>
						</xsl:for-each>
					</xsl:for-each>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ValidationAssociateRj">
		<font color="#000080">
			<b>A-ASSOCIATE-RJ Message</b>
			<br/>	Result: <xsl:for-each select="Result">
				<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
				<xsl:apply-templates/>
			</xsl:for-each>
			<br/>	Source: <xsl:for-each select="Source">
				<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
				<xsl:apply-templates/>
			</xsl:for-each>
			<br/>	Reason: <xsl:for-each select="Reason">
				<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
				<xsl:apply-templates/>
			</xsl:for-each>
		</font>
	</xsl:template>
	<xsl:template match="ValidationAbortRq">
		<font color="#000080">
			<b>A-ABORT Message</b>
			<br/>	Reason: <xsl:for-each select="Reason">
				<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
				<xsl:apply-templates/>
			</xsl:for-each>
			<br/>	Source: <xsl:for-each select="Source">
				<xsl:value-of select="@Value"/> (<xsl:value-of select="@Meaning"/>)
				<xsl:apply-templates/>
			</xsl:for-each>
		</font>
	</xsl:template>
	<xsl:template match="ValidationReleaseRq">
		<font color="#000080">
			<br/>
			<b>A-RELEASE-RQ Message</b>
			<xsl:apply-templates/>
		</font>
	</xsl:template>
	<xsl:template match="ValidationReleaseRp">
		<font color="#000080">
			<br/>
			<b>A-RELEASE-RP Message</b>
			<xsl:apply-templates/>
		</font>
	</xsl:template>
	<xsl:template match="ValidationResults">
		<font color="#000080">
			<xsl:apply-templates/>
		</font>
	</xsl:template>
	<xsl:template name="FindMessages">
		<xsl:for-each select="Message">
			<xsl:call-template name="ValidationMessages"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="ValidationCounters" name="ValidationCounters">
		<xsl:param name="filename">
			<xsl:value-of select="substring-before(/DvtDetailedResultsFile/DvtSummaryResultsFilename,'Summary')"/>
			<xsl:value-of select="substring-before(/DvtSummaryResultsFile/DvtDetailedResultsFilename,'Detail')"/>Summary<xsl:value-of select="substring-after(/DvtDetailedResultsFile/DvtSummaryResultsFilename,'Summary')"/>
			<xsl:value-of select="substring-after(/DvtSummaryResultsFile/DvtDetailedResultsFilename,'Detail')"/>
		</xsl:param>
		<xsl:for-each select="ValidationCounters">
			<xsl:for-each select="document($filterXMLfilename)/Nodes/added/Message">
				<xsl:if test="substring-before(Name,' * ')=$filename">
					<br/>
					<xsl:element name="a">
						<xsl:attribute name="href">file:///c:\ADD * <xsl:value-of select="Name"/></xsl:attribute>
						<font color="#FF0000">
							<br/>Manualy added Error: <xsl:value-of select="substring-after(Name,' * ')"/>
							<br/>
						</font>
					</xsl:element>(<xsl:element name="a">
						<xsl:attribute name="href">file:///c:\DEL * <xsl:value-of select="Name"/></xsl:attribute>Click here to delete error.</xsl:element>)<br/>
				</xsl:if>
			</xsl:for-each>
			<br/>
			<xsl:if test="ValidationTest[.='PASSED']">
				<font color="green">
					<i>
						<b>
						<br/>
					RESULT: <xsl:value-of select="ValidationTest"/>
							<br/>
							<xsl:for-each select="NrOfValidationErrors">
				
				Number of Validation Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfValidationWarnings">
				
					Number of Validation Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
						<br/>
						<b>
							<xsl:for-each select="NrOfUserErrors">
				
				Number of User Validation Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfUserWarnings">
				
					Number of User Validation Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
						<br/>
						<b>
							<xsl:for-each select="NrOfGeneralErrors">
				
				Number of General Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfGeneralWarnings">
				
					Number of General Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
					</i>
				</font>
			</xsl:if>
			<xsl:if test="not(ValidationTest[.='PASSED'])">
				<font color="#FF0000">
					<i>
						<b>
						<br/>
					RESULT: <xsl:value-of select="ValidationTest"/>
							<br/>
							<xsl:for-each select="NrOfValidationErrors">
				
				Number of Validation Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfValidationWarnings">
				
					Number of Validation Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
						<br/>
						<b>
							<xsl:for-each select="NrOfUserErrors">
				
				Number of User Validation Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfUserWarnings">
				
					Number of User Validation Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
						<br/>
						<b>
							<xsl:for-each select="NrOfGeneralErrors">
				
				Number of General Errors:	<xsl:value-of select="."/> - 
				
			</xsl:for-each>
							<xsl:for-each select="NrOfGeneralWarnings">
				
					Number of General Warnings:		
					<xsl:value-of select="."/>
							</xsl:for-each>
						</b>
					</i>
				</font>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="DirectoryRecord">
		<xsl:param name="ISSEQ"/>
		<br/>
		<b>DIRECTORY RECORD: <xsl:value-of select="@Index"/> (<xsl:value-of select="@Type"/>)</b>
		<br/>
		<xsl:for-each select="ValidationResults">
			<xsl:for-each select="ValidationDirectoryRecord">
				<table border="1" width="100%" cellpadding="3">
					<font color="#000080">
						<tr>
							<td align="center" valign="top" class="item" colspan="7">
								<b>	Directory Attributes </b>
							</td>
						</tr>
						<tr>
							<td valign="top" width="120" class="item">Attribute</td>
							<td valign="top" width="10" class="item">Def VR</td>
							<td valign="top" width="10" class="item">Dcm VR</td>
							<td valign="top" width="10" class="item">Type</td>
							<td valign="top" width="10" class="item">Pr</td>
							<td valign="top" width="10" class="item">Len</td>
							<td valign="top" width="140" class="item">Attribute Name</td>
							<td valign="top" class="item">Value(s) and Comments</td>
						</tr>
						<xsl:call-template name="VALAttributes">
							<xsl:with-param name="VALISSEQ">	</xsl:with-param>
						</xsl:call-template>
					</font>
				</table>
				<xsl:call-template name="FINDValidationMessages"/>
			</xsl:for-each>
			<xsl:call-template name="FINDValidationMessages"/>
		</xsl:for-each>
		<xsl:call-template name="FINDValidationMessages"/>
	</xsl:template>
	<xsl:template match="ReferencedFile">
		<xsl:for-each select="ValidationDirectoryRecord">
			<xsl:call-template name="objectresults"/>
		</xsl:for-each>
		<xsl:apply-templates/>
	</xsl:template>
	<xsl:template name="replace-string">
		<xsl:param name="text"/>
		<xsl:param name="from"/>
		<xsl:param name="to"/>
		<xsl:choose>
			<xsl:when test="contains($text, $from)">
				<xsl:variable name="before" select="substring-before($text, $from)"/>
				<xsl:variable name="after" select="substring-after($text, $from)"/>
				<xsl:variable name="prefix" select="concat($before, $to)"/>
				<xsl:value-of select="$before"/>
				<xsl:value-of select="$to"/>
				<br/>
				<xsl:call-template name="replace-string">
					<xsl:with-param name="text" select="$after"/>
					<xsl:with-param name="from" select="$from"/>
					<xsl:with-param name="to" select="$to"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$text"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="converttohtml">
		<xsl:param name="text"/>
		<xsl:choose>
			<xsl:when test="contains($text, '[')">
				<xsl:variable name="before" select="substring-before($text, '[')"/>
				<xsl:variable name="after" select="substring-after($text, '[')"/>
				<xsl:call-template name="converttohtml">
					<xsl:with-param name="text" select="$before"/>
				</xsl:call-template>
				<xsl:text disable-output-escaping="yes">&lt;</xsl:text>
				<xsl:call-template name="converttohtml">
					<xsl:with-param name="text" select="$after"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="contains($text, ']')">
						<xsl:variable name="before" select="substring-before($text, ']')"/>
						<xsl:variable name="after" select="substring-after($text, ']')"/>
						<xsl:call-template name="converttohtml">
							<xsl:with-param name="text" select="$before"/>
						</xsl:call-template>
						<xsl:text disable-output-escaping="yes">&gt;</xsl:text>
						<xsl:call-template name="converttohtml">
							<xsl:with-param name="text" select="$after"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$text"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="printmessage">
		<xsl:param name="type"/>
		<xsl:param name="index"/>
		<xsl:param name="message"/>
		<xsl:param name="messagetype"/>
		<xsl:param name="location"/>
		<xsl:param name="filename">
			<xsl:value-of select="/DvtDetailedResultsFile/DvtSummaryResultsFilename"/>
			<xsl:value-of select="/DvtSummaryResultsFile/DvtSummaryResultsFilename"/>
		</xsl:param>
		<xsl:param name="rule1">
			<xsl:value-of select="concat('ALL * ',$message)"/>
		</xsl:param>
		<xsl:param name="rule2">
			<xsl:value-of select="concat($filename,' * ',$message)"/>
		</xsl:param>
		<xsl:if test="(contains($skipped,$rule1) and $filter='yes')">
			<br/>
			<i>Note: Issue Ignored</i>
		</xsl:if>
		<xsl:if test="not(contains($skipped,$rule1) and $filter='yes')">
			<xsl:if test="(contains($skipped,$rule2) and $filter='yes')">
				<br/>
				<i>Note: Issue Ignored</i>
			</xsl:if>
			<xsl:if test="not(contains($skipped,$rule2) and $filter='yes')">
				<xsl:choose>
					<xsl:when test="$messagetype='HTMLUserActivity'">
						<xsl:choose>
							<xsl:when test="$type='Info'">
								<xsl:call-template name="converttohtml">
									<xsl:with-param name="text">
										<xsl:value-of select="$message"/>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:when test="$type='ConditionText'">
								<font color="#6699CC">ConditionText: <xsl:call-template name="converttohtml">
									<xsl:with-param name="text">
										<xsl:value-of select="$message"/>
									</xsl:with-param>
								</xsl:call-template></font>
							</xsl:when>
							<xsl:otherwise>
								<xsl:element name="a">
									<xsl:attribute name="href">file:///c:\<xsl:value-of select="$index"/> * <xsl:value-of select="$filename"/></xsl:attribute>
									<font color="#FF0000">
										<xsl:value-of select="$type"/>: 
     <xsl:call-template name="converttohtml">
											<xsl:with-param name="text">
												<xsl:value-of select="$message"/>
											</xsl:with-param>
										</xsl:call-template>
									</font>
								</xsl:element>
								<xsl:if test="count(/DvtSummaryResultsFile/DvtDetailedResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Detail<xsl:value-of select="substring-before(substring-after(/DvtSummaryResultsFile/DvtDetailedResultsFilename,'Detail'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Detailed Result
								</xsl:element>
								</xsl:if>
								<xsl:if test="count(/DvtDetailedResultsFile/DvtSummaryResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Summary<xsl:value-of select="substring-before(substring-after(/DvtDetailedResultsFile/DvtSummaryResultsFilename,'Summary'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Summary Result
								</xsl:element>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$messagetype='UserActivity'">
						<xsl:choose>
							<xsl:when test="$type='Info'">
								<xsl:call-template name="replace-string">
									<xsl:with-param name="text">
										<xsl:value-of select="$message"/>
									</xsl:with-param>
									<xsl:with-param name="from">[CR][LF]</xsl:with-param>
									<xsl:with-param name="to"/>
								</xsl:call-template>
							</xsl:when>
														<xsl:when test="$type='ConditionText'">
								<font color="#6699CC">ConditionText: <xsl:call-template name="replace-string">
									<xsl:with-param name="text">
										<xsl:value-of select="$message"/>
									</xsl:with-param>
									<xsl:with-param name="from">[CR][LF]</xsl:with-param>
									<xsl:with-param name="to"/>
								</xsl:call-template>></font>
							</xsl:when>
							<xsl:otherwise>
								<xsl:element name="a">
									<xsl:attribute name="href">file:///c:\<xsl:value-of select="$index"/> * <xsl:value-of select="$filename"/></xsl:attribute>
									<font color="#FF0000">
										<xsl:call-template name="replace-string">
											<xsl:with-param name="text">
												<xsl:value-of select="$message"/>
											</xsl:with-param>
											<xsl:with-param name="from">[CR][LF]</xsl:with-param>
											<xsl:with-param name="to"/>
										</xsl:call-template>
									</font>
								</xsl:element>
								<xsl:if test="count(/DvtSummaryResultsFile/DvtDetailedResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Detail<xsl:value-of select="substring-before(substring-after(/DvtSummaryResultsFile/DvtDetailedResultsFilename,'Detail'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Detailed Result
								</xsl:element>
								</xsl:if>
								<xsl:if test="count(/DvtDetailedResultsFile/DvtSummaryResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Summary<xsl:value-of select="substring-before(substring-after(/DvtDetailedResultsFile/DvtSummaryResultsFilename,'Summary'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Summary Result
								</xsl:element>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$type='Info'">
								<br/>
								<i>
									<xsl:value-of select="$message"/>
								</i>
							</xsl:when>
														<xsl:when test="$type='ConditionText'">
									<br/>
								<i>
									<font color="#6699CC">ConditionText: <xsl:value-of select="$message"/></font>
								</i>
							</xsl:when>
							<xsl:otherwise>
								<xsl:element name="a">
									<xsl:attribute name="NAME"><xsl:value-of select="$index"/></xsl:attribute>
									<xsl:attribute name="href">file:///c:\<xsl:value-of select="$index"/> * <xsl:value-of select="$filename"/></xsl:attribute>
									<font color="#FF0000">
										<br/>
										<xsl:value-of select="$type"/>: 
				<xsl:value-of select="$message"/>
									</font>
								</xsl:element>
								<xsl:if test="count(/DvtSummaryResultsFile/DvtDetailedResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Detail<xsl:value-of select="substring-before(substring-after(/DvtSummaryResultsFile/DvtDetailedResultsFilename,'Detail'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Detailed Result
								</xsl:element>
								</xsl:if>
								<xsl:if test="count(/DvtDetailedResultsFile/DvtSummaryResultsFilename)>0">
									<br/>
									<xsl:element name="a">
										<xsl:attribute name="href">Summary<xsl:value-of select="substring-before(substring-after(/DvtDetailedResultsFile/DvtSummaryResultsFilename,'Summary'),'.xml')"/>.xml#<xsl:value-of select="$index"/></xsl:attribute>
									Link to Summary Result
								</xsl:element>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
