using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VideoInfo : JdObject{


         [XmlElement("sid")]
public  		string
  sid { get; set; }


         [XmlElement("aka")]
public  		string
  aka { get; set; }


         [XmlElement("first_category")]
public  		int
  firstCategory { get; set; }


         [XmlElement("brand")]
public  		string
  brand { get; set; }


         [XmlElement("foreignname")]
public  		string
  foreignname { get; set; }


         [XmlElement("isbn")]
public  		string
  isbn { get; set; }


         [XmlElement("barcode")]
public  		string
  barcode { get; set; }


         [XmlElement("mvd_wxjz")]
public  		string
  mvdWxjz { get; set; }


         [XmlElement("mvd_gqyz")]
public  		string
  mvdGqyz { get; set; }


         [XmlElement("mvd_wyjz")]
public  		string
  mvdWyjz { get; set; }


         [XmlElement("isrc")]
public  		string
  isrc { get; set; }


         [XmlElement("mvd_dcz")]
public  		string
  mvdDcz { get; set; }


         [XmlElement("mvd_xcyg")]
public  		string
  mvdXcyg { get; set; }


         [XmlElement("press")]
public  		string
  press { get; set; }


         [XmlElement("publishing_company")]
public  		string
  publishingCompany { get; set; }


         [XmlElement("production_company")]
public  		string
  productionCompany { get; set; }


         [XmlElement("copyright")]
public  		string
  copyright { get; set; }


         [XmlElement("actor")]
public  		string
  actor { get; set; }


         [XmlElement("director")]
public  		string
  director { get; set; }


         [XmlElement("dub")]
public  		string
  dub { get; set; }


         [XmlElement("voiceover")]
public  		string
  voiceover { get; set; }


         [XmlElement("screenwriter")]
public  		string
  screenwriter { get; set; }


         [XmlElement("producer")]
public  		string
  producer { get; set; }


         [XmlElement("singer")]
public  		string
  singer { get; set; }


         [XmlElement("performer")]
public  		string
  performer { get; set; }


         [XmlElement("authors")]
public  		string
  authors { get; set; }


         [XmlElement("compose")]
public  		string
  compose { get; set; }


         [XmlElement("command")]
public  		string
  command { get; set; }


         [XmlElement("orchestra")]
public  		string
  orchestra { get; set; }


         [XmlElement("media")]
public  		string
  media { get; set; }


         [XmlElement("soundtrack")]
public  		int
  soundtrack { get; set; }


         [XmlElement("number_of_discs")]
public  		int
  numberOfDiscs { get; set; }


         [XmlElement("episode")]
public  		int
  episode { get; set; }


         [XmlElement("record_number")]
public  		int
  recordNumber { get; set; }


         [XmlElement("publication_date")]
public  		DateTime
  publicationDate { get; set; }


         [XmlElement("release_date")]
public  		DateTime
  releaseDate { get; set; }


         [XmlElement("accessories")]
public  		string
  accessories { get; set; }


         [XmlElement("included_additional_item")]
public  		int
  includedAdditionalItem { get; set; }


         [XmlElement("set_the_number_of")]
public  		int
  setTheNumberOf { get; set; }


         [XmlElement("format")]
public  		string
  format { get; set; }


         [XmlElement("color")]
public  		string
  color { get; set; }


         [XmlElement("region")]
public  		string
  region { get; set; }


         [XmlElement("length")]
public  		string
  length { get; set; }


         [XmlElement("screen_ratio")]
public  		string
  screenRatio { get; set; }


         [XmlElement("audio_encoding_chinese")]
public  		string
  audioEncodingChinese { get; set; }


         [XmlElement("quality_description")]
public  		string
  qualityDescription { get; set; }


         [XmlElement("dregion")]
public  		string
  dregion { get; set; }


         [XmlElement("language")]
public  		string
  language { get; set; }


         [XmlElement("language_dubbed")]
public  		string
  languageDubbed { get; set; }


         [XmlElement("language_subtitled")]
public  		string
  languageSubtitled { get; set; }


         [XmlElement("version_language")]
public  		string
  versionLanguage { get; set; }


         [XmlElement("language_pronunciation")]
public  		string
  languagePronunciation { get; set; }


         [XmlElement("menu_language")]
public  		string
  menuLanguage { get; set; }


         [XmlElement("version")]
public  		string
  version { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("platform")]
public  		string
  platform { get; set; }


         [XmlElement("minimum_system_requirement_description")]
public  		string
  minimumSystemRequirementDescription { get; set; }


         [XmlElement("recommended_system_description")]
public  		string
  recommendedSystemDescription { get; set; }


         [XmlElement("online_play_description")]
public  		string
  onlinePlayDescription { get; set; }


         [XmlElement("awards")]
public  		string
  awards { get; set; }


         [XmlElement("type_keywords")]
public  		string
  typeKeywords { get; set; }


         [XmlElement("keywords")]
public  		string
  keywords { get; set; }


         [XmlElement("readers")]
public  		string
  readers { get; set; }


         [XmlElement("number_of_players")]
public  		string
  numberOfPlayers { get; set; }


         [XmlElement("mfg_minimum")]
public  		int
  mfgMinimum { get; set; }


         [XmlElement("mfg_maximum")]
public  		int
  mfgMaximum { get; set; }


         [XmlElement("compile")]
public  		string
  compile { get; set; }


         [XmlElement("photography")]
public  		string
  photography { get; set; }


         [XmlElement("dictation")]
public  		string
  dictation { get; set; }


         [XmlElement("read")]
public  		string
  read { get; set; }


         [XmlElement("finishing")]
public  		string
  finishing { get; set; }


         [XmlElement("write")]
public  		string
  write { get; set; }


}
}
