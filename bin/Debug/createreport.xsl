<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="collection">
			<html>
			<head>

<META NAME="Classification" CONTENT="General HTML"/>

<META NAME="KEYWORDS" CONTENT="DICOM, Validation, Report, Results"/>

<META NAME="ABSTRACT" CONTENT="DICOM Validation Test Report"/>

<META NAME="description" CONTENT="This document contains the results of the DICOM Validation Test Spec."/>
          <title>DICOM Validation Test Report</title>
<link rel="stylesheet" href="base.css"/>
</head>
<body>
<p></p>

<hr/>

<table border="0" width="100%">
    <tr>
        <td> </td>
        <td align="center">
          <br/>
                    <p class="title">DICOM Validation Test Report</p>
          <p class="title"></p></td>
        <td></td>
    </tr>
</table>
<table border="0" width="100%">
    <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Vendor:</p>
        </td>
        <td valign="top" width="300">

</td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Tested System:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">DICOM</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description">Version:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">3.0</p></td>
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
          <p class="description">Date:</p>
        </td>
        <td valign="top" width="300">
          <p class="description">00/00/0000</p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="description"></p>
        </td>
        <td valign="top" width="300">
          <p class="description"></p></td>
        <td valign="top"></td>
    </tr>
        <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="item">Document Number:</p>
        </td>
        <td valign="top" width="300">
          <p class="item">XPR 080 - XXXXXX.XX</p></td>
        <td valign="top"></td>
           </tr>
                <tr>
        <td valign="top"></td>
        <td valign="top" width="200">
          <p class="item">Document Version:</p>
        </td>
        <td valign="top" width="300">
          <p class="item">1.0 (Proposal)</p></td>
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
        <td align="center"></td>
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
          of the test cases required to perform a DICOM Validation Test on any
          system supporting any of the DICOM SOP Classes, either as SCU or as
          SCP.<br/>
        </td>       
    </tr>    
</table>
<table border="0" width="100%">
    <tr>
        <td align="left"><br/>
          <p class="item">Test Approach</p></td>       
    </tr>
    <tr>
        <td align="left">In this test, not only the (normal) operation of the
          system (proper use of the protocol) is tested, but also potential side
          effects, wrong use of the protocol, reaction to deviations from the
          specification, etc (i.e. special cases to test the robustness of the
          implementation). The results are checked against the DICOM Standard
          and the Conformance Statement.<br/>
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
            <b>
            <p align="justify">F</p></b> - Failed
            <b>
            <p align="justify">P</p></b> - Passed
            <b>
            <p align="justify">N</p></b> - Not Relevant for this system
            <b>
            <p align="justify">X</p></b> - Not Tested (e.g. not testable at all,
            not tested because of available time, etc.)
          </dir>
          <p align="justify">A failure result categorized in minor and major
          issues. Minor issues are
          issues which will not influence the behavior of the system under
          normal operation, but need to be solved to behave according the DICOM
          Standard and/or to behave conform its own Conformance Statement.</p>
          <p align="justify">For the Conformance Statement, remarks can be given
          in the specially created table for this. Comments on the test cases
          (e.g. more detail about a concluded problem) can be given in the last
          column of the tables containing the test results of the system as SCU
          and SCP.</p>
        </td>       
    </tr>    
</table>

<hr/>
<h2>Found Problems</h2>

<table border="1" width="100%">
 <tr> <td valign="top" width="300" class="item">PR nr.</td>
<td valign="top" class="item">Description</td></tr>
<xsl:for-each select="//pr">
<xsl:if test="not(prnr=preceding::file/pr/prnr)">
    <tr>
        <td align="left"><xsl:value-of select="prnr"/></td>       
        <td align="left"><xsl:value-of select="com"/></td> 
    </tr>
    </xsl:if>   
</xsl:for-each>
</table>


<p/>
<xsl:for-each select="Role">
<h1>Overview of <xsl:value-of select="Rolename"/></h1><br/><br/>
<xsl:for-each select="Sop">

<table border="1" width="100%" cellpadding="3"> 
<tr> <td align="center" valign="top" class="item" colspan="4"> <p class="item">Overview of <xsl:value-of select="Sopname"/> Test Cases</p> </td> </tr>
 <tr> <td valign="top" width="300" class="item">Testcase</td> <td valign="top" width="150" class="item">Executed</td>
<td valign="top" width="150" class="item">Passed/Failed</td>
<td valign="top" class="item">Problems</td></tr>

<xsl:for-each select="file">
<tr><td valign="top" width="300"><b><xsl:value-of select="filename"/></b></td>
<td valign="top" width="150" class="item"><xsl:value-of select="Executed"/></td>
<td valign="top" width="150" class="item">
<xsl:if test="(Executed)='Y'">
<xsl:if test="count(problem)=0">P</xsl:if>
<xsl:if test="count(problem)>0">F</xsl:if></xsl:if>
<xsl:if test="(Executed)='N'">
.</xsl:if>

</td>
<td valign="top" class="item">.
<xsl:for-each select="prnr">
<xsl:if test="not(.='')"><xsl:value-of select="."/><br/></xsl:if>
</xsl:for-each>
</td></tr>
</xsl:for-each></table><br/><br/>
</xsl:for-each>
</xsl:for-each>

<table border="1" width="100%" cellpadding="3"> 
<tr> <td align="center" valign="top" class="item" colspan="2"> <p class="item">Problem Summary</p> </td> </tr>
 <tr> <td valign="top" width="300" class="item">Executed Scripts</td> <td valign="top" width="300" class="item">Number of problems found</td></tr>
<tr><td valign="top"><xsl:value-of select="count(//isexecuted)"/></td>
<td valign="top" width="300" class="item"><xsl:value-of select="count(//problem)"/></td>
</tr>
</table><br/><br/>

			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
