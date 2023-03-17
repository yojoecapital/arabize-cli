# Arabize

This is a command to translate Arabic-transliterated letters into Arabic Unicode characters and copy to clipboard.

To access it easier, place the path of the `Arabize` directory in `Path` for System Variables.

To recompile, use `csc arabize.cs`.

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
$ arabize lam_alif alif.h_lam_haa alif.h_lam_alif alif_lam_lam_haa
```

This copies this text:

> لا إله إلا الله

## How to Use Diacritics:

Use these sequences at the end of a transliterated letter to add a diacritic ligature to the Arabic Unicode letter.

| Sequence |        Diacritic        |
| :------: | :---------------------: |
|   `''`   |  &#x064B;  *fathatan*   |
|   `'`    |    &#x064E;  *fatha*    |
|   `--`   |  &#x064D;  *kasratan*   |
|   `-`    |    &#x0650;  *kasra*    |
|   `%%`   |  &#x064C;  *dammatan*   |
|   `%`    |    &#x064F;  *damma*    |
|   `$`    |   &#x0651;  *shadda*    |
|   `#`    |    &#x0652;  *sukun*    |
|   `@`    | &#x654;  *hamza above*  |
|   `.@`   | &#x655;  *hamza below*  |
|   `~`    | &#x653;  *maddah above* |

Note that diacritics also act as delimiters between letters. 

For example:

```
$ arabize alif.@-noon$'alif lam-lam$'haa- waw'alif.@-noon$'alif alif.@-lam'ya#haa- ra'alif_jeem-ayn%waw_noon'
```

Will copy this text:

> إِنَّا لِلَّهِ وَإِنَّا إِلَيْهِ رَاجِعُونَ

## How to Use Macros:

Use `arabize add inna alif.@-noon$'alif` to add إِنَّا in place of `inna`.

Use `arabize add-lit ArRahman الرَّحْمَان` to add الرَّحْمَان using its literal Unicode characters in place of `ArRahman`.

Use `arabize remove inna` to remove the macro `inna`.

Use `arabize macros` to get a list of all current macros.
