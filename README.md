# Arabize

This is a command to translate Arabic-transliterated letters into Arabic Unicode characters and copy to clipboard.

To use it, place the path of the `Arabize` directory in `Path` for System Variables.

To recompile, use `css arabize-script.c`.

## How to use:

Pass the transliterated letters to have the Arabic Unicode copied to clipboard.

```
C:\> arabize alif_lam_ra_ha_meem_alif_noon
```

This copies this text:

> الرحمان 

Use `_` characters to delimit letters. Use space characters to delimit words.

```
C:\> arabize lam_alif alif_lam_haa alif_lam_alif alif_lam_lam_haa
```

This copies this text:

> لا اله الا الله 

To add substitutions, edit the `arabic-letters.txt` file. The value goes on the left and the key goes on the right. The key is the transliterated letters whilst the values are the Arabic Unicode characters.

```
ا:alif
ب:ba
ت:ta
...
<value>:<key>
```

The keys must be unique or an error will be thrown when running `arabize`.