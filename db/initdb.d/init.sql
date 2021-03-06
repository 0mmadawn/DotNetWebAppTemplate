DROP SCHEMA IF EXISTS dev;
CREATE SCHEMA dev;
USE dev;

DROP TABLE IF EXISTS Users;

CREATE TABLE `Users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '名前',
  `group` tinyint unsigned NOT NULL DEFAULT '0' COMMENT '所属\\n1: エゥーゴ\\n2: ティターンズ\\n4: 地球連邦軍\\n8: アクシズ',
  `description` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL COMMENT '備考',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO `Users` (`name`,`group`,`description`) VALUES ('カミーユ・ビダン',1,'init test');
INSERT INTO `Users` (`name`,`group`,`description`) VALUES ('ファ・ユイリィ',1,'メタスに乗ってた');
INSERT INTO `Users` (`name`,`group`,`description`) VALUES ('ジェリド・メサ',2,'がんばれ!');
INSERT INTO `Users` (`name`,`group`,`description`) VALUES ('フォウ・ムラサメ',4,'私に優しくしてよ！');
INSERT INTO `Users` (`name`,`group`,`description`) VALUES ('ハマーン・カーン',8,'つよい');

