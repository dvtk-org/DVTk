<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:template match="DvtDetailedResultsFile">
		<xsl:param name="ISSEQ"/>
		<xsl:param name="RESULTTYPE"/>
		<xsl:param name="TYPE"/>
		<html>

<head>

<!-- Copyright (c) Philips Medical Systems - MIT Interoperability, www.philips.com, 2004. -->

          <title>SQA Test DICOM Validation Toolkit</title>
<link rel="stylesheet" href="../base.css"/>
</head>
<body>
<p>.</p>

<hr/>

<table border="0" width="100%">
    <tr>
        <td>.</td>
        <td align="center">
          <br/>
                    <p class="title">SQA Test DICOM Validation Toolkit</p>
          <p class="title"></p></td>
        <td>.</td>
    </tr>
</table>
<table border="0" width="100%">
    <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Vendor:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">www.DVTk.org</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Tested System:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">DVTk</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Version:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">2.1</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Tester:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">Name of tester</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="item"></p>
        </td>
        <td valign="top" width="300">
          <p class="item"></p></td>
        <td valign="top"></td>
    </tr>
</table>
<table border="0" width="100%">
    <tr>
            <td></td>
        <td align="center">
          <p class="sub">© Copyright 2005, <A HREF="mailto:validation@dvtk.org">www.DVTk.org</A>..</p></td>
        <td></td>
    </tr>
</table>

<hr/>

<table border="0" width="100%">
    <tr>
        <td align="left"><br/>
          <p class="item">Introduction</p></td>       
    </tr>
    <tr>
        <td align="left">The purpose of this document is to report the results
          of the test cases required to perform the SQA test on the DICOM Validation Toolkit.<br/>
        </td>       
    </tr>    
</table>
<table border="0" width="100%">
    <tr>
        <td align="left"><br/>
          <p class="item">Test Approach</p></td>       
    </tr>
    <tr>
        <td align="left">In this test, not only validation mechanism of the DVTk (DICOM Validation Toolkitt is tested.<br/>
        </td>       
    </tr>    
</table>
<table border="0" width="100%">
    <tr>
        <td align="left"><br/>
          <p class="item">Test Results</p></td>       
    </tr>
    <tr>
        <td align="left">
          <p align="justify">The tables in the following sections contain all
          test cases, including the results. The following conventions will be
          used for each test case result:</p>
          <dir>
            
            <p align="justify"><b>F</b> - Failed</p>
            
            <p align="justify"><b>P</b> - Passed</p>

            <p align="justify"><b>N</b> - Not Relevant for this system</p>

            <p align="justify"><b>X</b> - Not Tested (e.g. not testable at all,
            not tested because of available time, etc.)</p>
          </dir>
        </td>       
    </tr>    
</table>

<hr/>

<table border="1" width="100%">
<tr>
	<td>Session</td>
<td>Testcase</td>
<td>P/F</td>
<td>Comment</td>
</tr>
<xsl:for-each select="//UserActivity">
		<xsl:if test="@Level[.='Error']">
     <xsl:call-template name="replace-string">
        <xsl:with-param name="text"><xsl:value-of select="."/></xsl:with-param> 
        </xsl:call-template>
		</xsl:if>
		<xsl:if test="@Level[.='Info']">
     <xsl:call-template name="replace-string">
<xsl:with-param name="text"><xsl:value-of select="."/></xsl:with-param> 
</xsl:call-template>
        	</xsl:if>
		<xsl:if test="@Level[.='Warning']">
			    <xsl:call-template name="replace-string">
 <xsl:with-param name="text"><xsl:value-of select="."/></xsl:with-param> 
 </xsl:call-template>
		</xsl:if>

</xsl:for-each>
				</table>
				
				<xsl:for-each select="ValidationCounters">
		<br/>
		<br/>
<xsl:if test="ValidationTest[.='PASSED']">
		<font color="green">
			<i>
				<b>
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

<hr/>


<br/><br/>
<table border="0" width="100%"> <tr> <td>.</td> <td align="center"> <p class="sub">© Copyright 2005
, <A HREF="mailto:validation@dvtk.org">SQA - www.DVTk.org</A>..</p></td> <td>.</td> </tr> </table> </body> </html>
	</xsl:template>
	<xsl:template match="SessionDetails">
	</xsl:template>
	<xsl:template match="UserActivity">
	</xsl:template>
 <xsl:template name="replace-string">
    <xsl:param name="text"/>

    <xsl:choose>
      <xsl:when test="contains($text, '[')">

	<xsl:variable name="before" select="substring-before($text, '[')"/>
	<xsl:variable name="after" select="substring-after($text, '[')"/>

       <xsl:call-template name="replace-string">
	  <xsl:with-param name="text" select="$before"/>
	</xsl:call-template>
<xsl:text disable-output-escaping="yes">&lt;</xsl:text>
        <xsl:call-template name="replace-string">
	  <xsl:with-param name="text" select="$after"/>
	</xsl:call-template>
      </xsl:when> 
      <xsl:otherwise>
    <xsl:choose>
      <xsl:when test="contains($text, ']')">

	<xsl:variable name="before" select="substring-before($text, ']')"/>
	<xsl:variable name="after" select="substring-after($text, ']')"/>

       <xsl:call-template name="replace-string">
	  <xsl:with-param name="text" select="$before"/>
	</xsl:call-template>

      <xsl:text disable-output-escaping="yes">&gt;</xsl:text>
        <xsl:call-template name="replace-string">
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

</xsl:stylesheet>
