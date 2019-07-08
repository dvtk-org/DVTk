<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dimse="urn:schemas-dvtk:dimse" xmlns:activity="urn:schemas-dvtk:activity" xmlns:validation="urn:schemas-dvtk:validation" xmlns:media="urn:schemas-dvtk:media" xmlns:dul="urn:schemas-dvtk:dul" xmlns:results="urn:schemas-dvtk:results" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:key name="location_by_mserr" match="Error/result" use="message"/>
	<xsl:key name="location_by_mswrn" match="Warning/result" use="message"/>
	<xsl:key name="location_by_msinfo" match="Info/result" use="message"/>
	<xsl:template match="collection">
		<collection>
			<xsl:apply-templates/>
		<xsl:for-each select="Directory">
			 <Directory><xsl:value-of select="."/></Directory>
		</xsl:for-each>
						<xsl:for-each select="FileName">
						 <FileName><xsl:value-of select="."/></FileName>
		</xsl:for-each>
		</collection>
	</xsl:template>
	<xsl:template match="Error">
		<Error>
			<xsl:for-each select="result[count(. | key('location_by_mserr',message)[1]) = 1]">
				<xsl:sort select="message"/>
				<message>
					<xsl:attribute name="Name"><xsl:value-of select="message"/></xsl:attribute>
					<xsl:for-each select="key('location_by_mserr',message)">
						<results>
								<id>
									<xsl:value-of select="id"/>
								</id>
							<Resultfile>
								<xsl:value-of select="Resultfile"/>
							</Resultfile>
						</results>
					</xsl:for-each>
				</message>
			</xsl:for-each>
		</Error>
	</xsl:template>
	<xsl:template match="Warning">
		<Warning>
			<xsl:for-each select="result[count(. | key('location_by_mswrn',message)[1]) = 1]">
				<xsl:sort select="message"/>
				<message>
					<xsl:attribute name="Name"><xsl:value-of select="message"/></xsl:attribute>
					<xsl:for-each select="key('location_by_mswrn',message)">
						<results>
														<id>
									<xsl:value-of select="id"/>
								</id>
							<Resultfile>
								<xsl:value-of select="Resultfile"/>
							</Resultfile>
						</results>
					</xsl:for-each>
				</message>
			</xsl:for-each>
		</Warning>
	</xsl:template>
</xsl:stylesheet>
