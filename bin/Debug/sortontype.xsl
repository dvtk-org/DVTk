<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dimse="urn:schemas-dvtk:dimse" xmlns:activity="urn:schemas-dvtk:activity" xmlns:validation="urn:schemas-dvtk:validation" xmlns:media="urn:schemas-dvtk:media" xmlns:dul="urn:schemas-dvtk:dul" xmlns:results="urn:schemas-dvtk:results" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:key name="message_by_type" match="Result" use="type"/>
	<xsl:template match="collection">
		<collection>
			<xsl:for-each select="Resultfile/Result[count(. | key('message_by_type',type)[1]) = 1]">
				<xsl:sort select="type"/>
				<xsl:if test="type[.='Warning']">
					<Warning>
						<xsl:for-each select="key('message_by_type', type)">
							<result>
								<message>
									<xsl:value-of select="message"/>
								</message>
								<id>
									<xsl:value-of select="id"/>
								</id>
																<Resultfile>
									<xsl:value-of select="../filename"/>
								</Resultfile>
							</result>
						</xsl:for-each>
					</Warning>
				</xsl:if>
				<xsl:if test="type[.='Error']">
					<Error>
						<xsl:for-each select="key('message_by_type', type)">
							<result>
								<message>
									<xsl:value-of select="message"/>
								</message>
																<id>
									<xsl:value-of select="id"/>
								</id>
																<Resultfile>
									<xsl:value-of select="../filename"/>
								</Resultfile>
							</result>
						</xsl:for-each>
					</Error>
				</xsl:if>
				<xsl:if test="type[.='Info']">
					<Info>
						<xsl:for-each select="key('message_by_type', type)">
							<result>
								<message>
									<xsl:value-of select="message"/>
								</message>
																<id>
									<xsl:value-of select="id"/>
								</id>
																<Resultfile>
									<xsl:value-of select="../filename"/>
								</Resultfile>
							</result>
						</xsl:for-each>
					</Info>
				</xsl:if>
			</xsl:for-each>
			<xsl:for-each select="Directory">
			 <Directory><xsl:value-of select="."/></Directory>
		</xsl:for-each>
						<xsl:for-each select="FileName">
						 <FileName><xsl:value-of select="."/></FileName>
		</xsl:for-each>
		</collection>
	</xsl:template>
</xsl:stylesheet>
