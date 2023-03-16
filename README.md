# Arabize

This is a command to translate Arabic-transliterated letters into Arabic Unicode characters and copy to clipboard.

To access it easier, place the path of the `Arabize` directory in `Path` for System Variables.

To recompile, use `csc arabize-script.c`.

## How to use:

Use `arabize letters` to get a list of all transliterated letters and their Unicode mappings.

Pass the transliterated letters to have the Arabic Unicode copied to clipboard.

```
$ arabize alif_lam_ra_ha_meem_alif_noon
```

This copies this text:

> الرحمان 

Use `_` characters to delimit letters. Use space characters to delimit words.

```
$ arabize lam_alif alif_lam_haa alif_lam_alif alif_lam_lam_haa
```

This copies this text:

> لا اله الا الله 

## How to Use Diacritics:

Use these sequences to the end of a transliterated letter to add a diacritic to the Arabic Unicode letter.

| Sequence | Diacritic |
| :------: | :-------: |
|   `''`   | &#x064B;  |
|   `'`    | &#x064E;  |
|   `--`   | &#x064D;  |
|   `-`    | &#x0650;  |
|   `%%`   | &#x064C;  |
|   `%`    | &#x064F;  |
|   `$`    | &#x0651;  |
|   `#`    | &#x0652;  |

For example:

```
$ arabize alif-h-_noon$_alif lam-_lam$_haa- waw' alif-h-_noon$_alif alif-h-_lam_yah#_haa- ra'_alif_jeem-_ayn%_waw_noon'
```

Will copy this text:

> إِنّا لِلّهِ وَ إِنّا إِليْهِ رَاجِعُونَ

## How to Use Macros:

Use `arabize add laa lam_alim` to add لا in place of `laa`.

Use `arabize add-lit laa لا ` to add لا using its literal Unicode characters in place of `laa`.

Use `arabize remove laa` to remove the macro.

Use `arabize macros` to get a list of all current macros.
