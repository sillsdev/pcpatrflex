----------------------------------------------------------------------------------------------------
|                                                                                                  |
|                                    TonePars control File info                                    |
|                                                                                                  |
----------------------------------------------------------------------------------------------------
| Jan/1999  Andy Black
| Apr/1999  R. Fumey
| Aug/2006  R. Fumey
| May/2008  R. Fumey
| Oct/2010  R. Fumey

|Tone patterns to be added:
| OBL'A'EO'D, OBL'A'EO'Q

|{ DEFINITIONS
|\segments KVGTP.SEG
\segments C:\Users\AndyBl~1\Docume~1\FieldW~1\PcPatr~1\Disamb~2\TestData\KVGTP.SEG
|\segments "E:\!Data\LangWork\Kuni\Carla\KVG Control Files\KVGTP.SEG"

                        | file defining what the segments are and their
                        |  relative sonority values

\monomoraic             | all syllables are monomoraic

\phrasefinalchars  .,;:!?"“”    | Need to know which punctuation characters
                                | constitute the end of a phrase

                        | Tone "types" or "status"; a "D" means that the
                        | status is used in showing Derivations; an "O"
                        | means that the status is used in showing Orthographic
                        | forms
\tonetype linked                D O
\tonetype floating              D
\tonetype left-floating         D
\tonetype right-floating        D
\tonetype boundary
\tonetype delinked

                        | tone values; primary  (\tonevalue) and
                        |              register (\tone_reg_value)
\tonevalue H    | high
\tonevalue M    | mid
\tonevalue R    | rising
\tonevalue L    | low
\tonevalue F    | rising/falling, falling
\tonevalue N    | none

                        | tone (morpho-phonological) domains
\tonedomain Word        | this is not implemented yet

\default DIRECTION: rightward
\default CYCLE: right-to-left

| \default CYCLE: left-to-right
|}
|***************************************************************************************************
|                       TONE RULES
|***************************************************************************************************
\begcycle               | cyclic rules
|{ A)  ########## TONE RULES FOR VERBAL CLAUSES - NON-COMPLEMENTATIONAL ############################

|[ A1) ********** Subordinative Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SUBORDINATIVE Tone Pattern 																   «SUB»
| marked by:
| - No tone mark on the whole clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

| Does not need to be defined here


|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SUBORDINATIVE EMPHATIC OBJECT Tone Pattern 												«SUB'EO»
| marked by:
| - Mid tone on the last syllable of the emphatic object preceding the verb¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

| Might not need to be defined here

|]
|[ A2) ********** Non-Intentive Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern 									 «NINT'FN'D»
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus¹
| - Optional Mid tone on the last syllable of the noun phrase in focus¹
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FN'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FN'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern 									 «NINT'FN'Q»
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus
|    OR
| - Mid tone on the last syllable of the NP in focus (i.e. immediately preceding the verb)
| - High tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FN'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'FN'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern 									 «NINT'FV'D»
| marked by:
| - Low Tone on the first verb syllable
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FV'D_TR
        Associate a L tone at left edge of word,
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FV'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern 								 «NINT'FV'Q»
| marked by:
| - Optional Low tone on the first verb syllable, if the verb has a multisyllabic prefix,
|   otherwise on the last syllable preceding the verb¹
| - High Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FV'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'FV'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule NINT'FV'Q_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FV'Q»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern 								 «NINT'EO'D»
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - High tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'EO'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'EO'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE EMPHATIC-OBJECT INTERROGATIVE Tone Pattern 								 «NINT'EO'Q»
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Rising/falling tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)¹
| - High tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'EO'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'EO'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE EMPHATIC-DEMONSTRATIVE-PRECEDING-THE-VERB DECLARATIVE Tone Pattern 		«NINT'EDP'D»
| marked by:
| - High tone on the first syllable of the demonstrative preceding the verb.¹
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'EDP'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'EDP'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE EMPHATIC-DEMONSTRATIVE-PRECEDING-THE-VERB INTERROGATIVE Tone Pattern 	«NINT'EDP'Q»
| marked by:
| - Rising/falling tone on the first syllable of the demonstrative preceding the verb.¹
| - High Tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'EDP'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'EDP'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-NP EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB DECLARATIVE Tone Pattern
|																					 «NINT'FN'EDF'D»
| marked by:
| - High Tone on the last verb syllable
| - High tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FN'EDF'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FN'EDF'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-NP EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB INTERROGATIVE Tone Pattern
|																					 «NINT'FN'EDF'Q»
| marked by:
| - Mid tone on the last syllable of the NP in focus (i.e. immediately preceding the verb)¹
| - High Tone on the last syllable preceding the verb stem²
| - Mid tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FN'EDF'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'FN'EDF'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-VERB EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB DECLARATIVE Tone Pattern
|																					 «NINT'FV'EDF'D»
| marked by:
| - Optional Low tone on the first syllable of the verb
| - High Tone on the last verb syllable
| - High tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FV'EDF'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FV'EDF'D»)
           )

| treatment of optional tone:
\tone_rule NINT'FV'EDF'D_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «NINT'FV'EDF'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| NON-INTENTIVE FOCUS-ON-VERB EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB INTERROGATIVE Tone Pattern
|																					 «NINT'FV'EDF'Q»
| marked by:
| - High Tone on the last syllable preceding the verb stem²
| - Mid tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule NINT'FV'EDF'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «NINT'FV'EDF'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|]
|[ A3) ********** Intentive Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern 										  «INT'FN'D»
| marked by:
| - High Tone on the last syllable before the verb stem
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FN'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FN'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern 										  «INT'FN'Q»
| marked by:
| - High Tone on the last syllable before the verb stem
|   (where absolutive prefixes are counted as belonging to the verb stem)
| - Middle Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FN'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FN'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern 										  «INT'FV'D»
| marked by:
| - Optional Low Tone on first verb syllable
| - High Tone on the last syllable before the verb stem²
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FV'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule INT'FV'D_a_TR (optional)
        Associate a L tone at left edge of word,
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern 									  «INT'FV'Q»
| marked by:
| - Optional Low Tone on first verb syllable
| - High Tone on the last syllable before the verb stem²
| - Middle Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FV'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule INT'FV'Q_a_TR (optional)
        Associate a L tone at left edge of word,
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern 									  «INT'EO'D»
| marked by:
| - High Tone on the last syllable before the verb stem²
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'EO'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'EO'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE EMPHATIC-OBJECT INTERROGATIVE Tone Pattern 									  «INT'EO'Q»
| marked by:
| - High Tone on the last syllable before the verb stem²
| - Middle Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'EO'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'EO'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE EMPHATIC-DEMONSTRATIVE-PRECEDING-THE-VERB DECLARATIVE Tone Pattern 			 «INT'EDP'D»
| marked by:
| - High tone on the first syllable of the demonstrative preceding the verb.¹
| - High tone on the last syllable preceding the verb stem²
| - High tone on the last syllable of the verb
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'EDP'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'EDP'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE EMPHATIC-DEMONSTRATIVE-PRECEDING-THE-VERB INTERROGATIVE Tone Pattern 		 «INT'EDP'Q»
| marked by:
| - Rising/falling tone on the first syllable of the demonstrative preceding the verb.¹
| - High Tone on the last syllable preceding the verb stem²
| - Middle Tone on the last syllable of the verb
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'EDP'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'EDP'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-NP EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB DECLARATIVE Tone Pattern
|																					  «INT'FN'EDF'D»
| marked by:
| - High tone on the last syllable preceding the verb stem²
| - High tone on the last syllable of the verb
| - High tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FN'EDF'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FN'EDF'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-NP EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB INTERROGATIVE Tone Pattern
|																					  «INT'FN'EDF'Q»
| marked by:
| - Mid tone on the last syllable of the NP in focus (i.e. immediately preceding the verb)¹
| - High Tone on the last syllable preceding the verb stem²
| - Middle Tone on the last syllable of the verb
| - Mid tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FN'EDF'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FN'EDF'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-VERB EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB DECLARATIVE Tone Pattern
|																					  «INT'FV'EDF'D»
| marked by:
| - Optional Low Tone on first verb syllable
| - High tone on the last syllable preceding the verb stem²
| - High tone on the last syllable of the verb
| - High tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FV'EDF'D_TR
        Associate a H tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'EDF'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule INT'FV'EDF'D_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «INT'FV'EDF'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| INTENTIVE FOCUS-ON-VERB EMPHATIC-DEMONSTRATIVE-FOLLOWING-THE-VERB INTERROGATIVE Tone Pattern
|																					  «INT'FV'EDF'Q»
| marked by:
| - Optional Low Tone on first verb syllable
| - High Tone on the last syllable preceding the verb stem²
| - Middle Tone on the last syllable of the verb
| - Mid tone on the first syllable of the demonstrative following the verb.¹
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule INT'FV'EDF'Q_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «INT'FV'EDF'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule INT'FV'EDF'Q_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «INT'FV'EDF'Q»)
           )
|]
|[ A4) ********** Sequential Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL1 NON-INTENTIVE Tone Pattern 												 «SEQ1'NINT»
| marked by:
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ1'NINT_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «SEQ1'NINT»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL1 NON-INTENTIVE with EMPHATIC OBJECT Tone Pattern 						  «SEQ1'NINT'EO»
| marked by:
| - Mid tone on the last syllable of the emphatic object preceding the verb¹
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ1'NINT'EO_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «SEQ1'NINT'EO»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL1 INTENTIVE Tone Pattern 													  «SEQ1'INT»
| marked by:
| - High Tone on the last syllable before the verb stem
|   (where absolutive prefixes are counted as belonging to the verb stem)
| - Middle Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ1'INT_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «SEQ1'INT»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL1 INTENTIVE with EMPHATIC OBJECT Tone Pattern 							   «SEQ1'INT'EO»
| marked by:
| - Mid tone on the last syllable of the emphatic object preceding the verb¹
| - High Tone on the last syllable before the verb stem²
| - Middle Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ1'INT'EO_TR
        Associate a M tone at right edge of word,
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «SEQ1'INT'EO»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL2 NON-INTENTIVE Tone Pattern (Variant 1) 									«SEQ2'NINT¹»
| marked by:
| - No tone mark on the whole clause (always follows ‹sasa›)
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

| Does not need to be defined here

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL2 NON-INTENTIVE Tone Pattern (Variant 2) 									«SEQ2'NINT²»
| marked by:
| - Mid tone marks on all the verb vowels
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ2'NINT²_TR
        Associate a M tone at first syllable of word,
        Associate a M tone at second syllable of word,
        Associate a M tone at third syllable of word,
        Associate a M tone at antepenultimate syllable of word,
        Associate a M tone at penultimate syllable of word,
        Associate a M tone at ultimate syllable of word.

        CONDITION:
           (
           (current morphname is «SEQ2'NINT²»)
           )


|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| SEQUENTIAL2 INTENTIVE Tone Pattern (Variant 1) 										  «SEQ2'INT»
| marked by:
| - No tone mark on the whole clause (always follows ‹sasa›)
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule SEQ2'INT_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «SEQ2'INT»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|]
|[ A5) ********** Obligative Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE FOCUS-ON-NP DECLARATIVE Tone Pattern				  «OBL'A'FN'D»
| (identical to NON-INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern)
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus¹
| - Optional Mid tone on the last syllable of the noun phrase in focus¹
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'FN'D≡NINT'FN'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «OBL'A'FN'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern				  «OBL'A'FN'Q»
| (identical to NON-INTENTIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern)
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus
|    OR
| - Mid tone on the last syllable of the NP in focus (i.e. immediately preceding the verb)
| - High tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'FN'Q≡NINT'FN'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «OBL'A'FN'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern				  «OBL'A'FV'D»
| (identical to NON-INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern)
| marked by:
| - Low Tone on the first verb syllable
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'FV'D≡NINT'FV'D_TR
        Associate a L tone at left edge of word,
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «OBL'A'FV'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern			  «OBL'A'FV'Q»
| (identical to NON-INTENTIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern)
| marked by:
| - Optional Low tone on the first verb syllable, if the verb has a multisyllabic prefix,
|   otherwise on the last syllable preceding the verb¹
| - High Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'FV'Q≡NINT'FV'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «OBL'A'FV'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule OBL'A'FV'Q≡NINT'FV'Q_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «OBL'A'FV'Q»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern			  «OBL'A'EO'D»
| (identical to NON-INTENTIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern)
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - High tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'EO'D≡NINT'EO'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «OBL'A'EO'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE EMPHATIC-OBJECT INTERROGATIVE Tone Pattern			  «OBL'A'EO'Q»
| (identical to NON-INTENTIVE EMPHATIC-OBJECT INTERROGATIVE Tone Pattern)
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Rising/falling tone on the last syllable of the emphatic noun phrase¹
| - High tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'EO'Q≡NINT'EO'Q_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «OBL'A'EO'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )


|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE SEQUENTIAL Tone Pattern 							   «OBL'A'SEQ»
| (identical to SEQUENTIAL1 NON-INTENTIVE Tone Pattern)
| marked by:
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'SEQ≡SEQ1'NINT_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «OBL'A'SEQ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE AFFIRMATIVE SEQUENTIAL with EMPHATIC OBJECT Tone Pattern 	 «OBL'A'SEQ'EO»
| (identical to SEQUENTIAL1 NON-INTENTIVE with EMPHATIC OBJECT Tone Pattern)
| marked by:
| - Mid tone on the last syllable of the emphatic object preceding the verb¹
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'A'SEQ'EO≡SEQ1'NINT'EO_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «OBL'A'SEQ'EO»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE NEGATIVE Tone Pattern 															 «OBL'N»
| marked by:
| - Low Tone on the penultimate verb syllabe
| - Rising Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'N_TR
        Associate a L tone at penultimate syllable of word,
        Associate a R tone at right edge of word.

        CONDITION:
           (
           (current morphname is «OBL'N»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| OBLIGATIVE NEGATIVE with EMPHATIC OBJECT Tone Pattern 								  «OBL'N'EO»
| marked by:
| - Mid tone on the last syllable of the emphatic object¹
| - Low Tone on the penultimate verb syllabe
| - Rising Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule OBL'N'EO_TR
        Associate a L tone at penultimate syllable of word,
        Associate a R tone at right edge of word.

        CONDITION:
           (
           (current morphname is «OBL'N'EO»)
           )

|]
|[ A6) ********** Existential, Deictic and Attention-Drawing Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EXISTENTIAL Tone Pattern																	>«EXIST»
| marked by:
| - Falling Tone on the penultimate syllabe
| - High Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EXIST_TR
        Associate a H tone at right edge of word,
        Associate a F tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «EXIST»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| DEICTIC INTERROGATIVE Tone Pattern														 >«DX'Q»
| marked by:
| - Rising/falling Tone on the last syllable of the emphatic object preceding the verb¹
| - Rising/falling Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule DX'Q_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (
           (current morphname is «DX'Q»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| ATTENTION-DRAWING DECLARATIVE Tone Pattern											   «ATTDR'D»
| marked by:
| - High Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule ATTDR'D_TR
        Associate a H tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «ATTDR'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| ATTENTION-DRAWING INTERROGATIVE Tone Pattern											   «ATTDR'Q»
| marked by:
| - Rising/falling Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule ATTDR'Q_TR
        Associate a F tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «ATTDR'Q»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|]
|}
|{ B)  ########## TONE RULES FOR VERBAL CLAUSES - COMPLEMENTATIONAL ################################

|[ B1) ********** Complementational Subordinative Tone Patterns **********

| none
|]
|[ B2) ********** Complementational Non-Intentive Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern 				«CMPL"NINT'FN'D»
| marked by:
| - Mid tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus¹
| - Optional Mid tone on the last syllable of the noun phrase in focus¹
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'FN'D_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (
           (current morphname is «CMPL"NINT'FN'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern 		 «CMPL"NINT'FN'Q¦Dᴮ»
| and COMPLEMENTATIONAL2 NON-INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the noun phrase in focus
|    OR
| - Mid tone on the last syllable of the NP in focus (i.e. immediately preceding the verb)
| - Mid tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'FN'Q¦Dᴮ_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"NINT'FN'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern			«CMPL"NINT'FV'D»
| marked by:
| - Low Tone on the first verb syllable
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'FV'D_TR
        Associate a L tone at left edge of word,
        Associate a M tone at right edge of word.

        CONDITION:
           (
           (current morphname is «CMPL"NINT'FV'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern 		 «CMPL"NINT'FV'Q¦Dᴮ»
| and COMPLEMENTATIONAL2 NON-INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern
| marked by:
| - Optional Low tone on the first verb syllable, if the verb has a multisyllabic prefix,
|   otherwise on the last syllable preceding the verb¹
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'FV'Q¦Dᴮ_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"NINT'FV'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule CMPL"NINT'FV'Q¦Dᴮ_a_TR (optional)
        Associate a L tone at left edge of word.

        CONDITION:
           (
           (current morphname is «CMPL"NINT'FV'Q¦Dᴮ»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern 			«CMPL"NINT'EO'D»
| marked by:
| - Mid tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Mid tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'EO'D_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (
           (current morphname is «CMPL"NINT'EO'D»)
           )


|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL NON-INTENTIVE EMPHATIC-OBJECT INTERROGATIVE Tone Pattern	 «CMPL"NINT'EO'Q¦Dᴮ»
| and COMPLEMENTATIONAL2 NON-INTENTIVE EMPHATIC-OBJECT DECLARATIVE Tone Pattern
| marked by:
| - Mid tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Mid tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)¹
| - Mid tone on the last syllable preceding the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"NINT'EO'Q¦Dᴮ_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"NINT'EO'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|]
|[ B3) ********** Complementational Intentive Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern 					 «CMPL"INT'FN'D»
| marked by:
| - Mid Tone on the last syllable before the verb stem
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"INT'FN'D_TR
        Associate a M tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FN'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL INTENTIVE FOCUS-ON-NP INTERROGATIVE Tone Pattern 			  «CMPL"INT'FN'Q¦Dᴮ»
| and COMPLEMENTATIONAL2 INTENTIVE FOCUS-ON-NP DECLARATIVE Tone Pattern
| marked by:
| - Mid Tone on the last syllable before the verb stem
|   (where absolutive prefixes are counted as belonging to the verb stem)
| - Low Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"INT'FN'Q¦Dᴮ_TR
        Associate a L tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FN'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL INTENTIVE FOCUS-ON-VERB DECLARATIVE Tone Pattern 				 «CMPL"INT'FV'D»
| marked by:
| - Optional Low Tone on first verb syllable
| - Mid Tone on the last syllable before the verb stem²
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"INT'FV'D_TR
        Associate a M tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FV'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule CMPL"INT'FV'D_a_TR (optional)
        Associate a L tone at left edge of word,
        Associate a M tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FV'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL INTENTIVE FOCUS-ON-VERB INTERROGATIVE Tone Pattern 				 «CMPL"INT'FV'Q»
| marked by:
| - Optional Low Tone on first verb syllable
| - Mid Tone on the last syllable before the verb stem²
| - Low Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"INT'FV'Q¦Dᴮ_TR
        Associate a L tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FV'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

| treatment of optional tone:
\tone_rule CMPL"INT'FV'Q¦Dᴮ_a_TR (optional)
        Associate a L tone at left edge of word,
        Associate a L tone at right edge of word,
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"INT'FV'Q¦Dᴮ»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )
|]
|[ B6) ********** Complementational Existential and Attention-Drawing Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL EXISTENTIAL Tone Pattern											«CMPL"EXIST»
| marked by:
| - Falling Tone on the penultimate syllabe
| - Mid Tone on the last verb syllable
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"EXIST_TR
        Associate a M tone at right edge of word,
        Associate a F tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «CMPL"EXIST»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL ATTENTION-DRAWING DECLARATIVE Tone Pattern						  «CMPL"ATTDR'D»
| marked by:
| - Mid Tone on the last syllable before the verb stem²
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"ATTDR'D_TR
        Associate a M tone at right edge of morpheme.

        CONDITION:
           (    (final morphname is «CMPL"ATTDR'D»)
            AND (   (    (right type is root)
                     AND (current type is prefix)
                     AND NOT (current property is Ac2)
                    )
                 OR (    (current type is prefix)
                     AND (right property is Ac2)
                     AND NOT (current property is Ac2)
                    )
                )
           )

|]
|}
|{ C)  ########## TONE RULES FOR NON-VERBAL CLAUSES - NON-COMPLEMENTATIONAL ########################

|[ C1) ********** Equative Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE DECLARATIVE Tone Pattern 														  «EQ'D»
| marked by:
| - High tone on any relevant possessive marker(s) (if existing)¹
| - High tone on the last syllable of the clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (current morphname is «EQ'D»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE INTERROGATIVE Tone Pattern 														  «EQ'Q»
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing)¹
| - Rising/falling tone on the last syllable of the clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'Q_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (current morphname is «EQ'Q»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE with EMPHATIC OBJECT DECLARATIVE Tone Pattern 								   «EQ'EO'D»
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - High tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)
| - High tone on the last syllable of the clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'EO'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (current morphname is «EQ'EO'D»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE with EMPHATIC OBJECT INTERROGATIVE Tone Pattern 								   «EQ'EO'Q»
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Rising/falling tone on the last syllable of the emphatic noun phrase (which immediately precedes the verb)
| - Rising/falling tone on the last syllable of the clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'EO'Q_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (current morphname is «EQ'EO'Q»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE with EMPHATIC OBJECT DECLARATIVE Tone Pattern						 «EQ'N'EO'D»
| marked by:
| - High tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - High tone on the last syllable preceding the particle ‹mba›
| - High tone on the particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'N'EO'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «EQ'N'EO'D»)
           )

| In the case of ‹nakémbá› and ‹khapémbá›:
\tone_rule EQ'N'EO'D_TR_a_TR (optional)
        Associate a H tone at right edge of word,
        Associate a H tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «EQ'N'EO'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE with EMPHATIC OBJECT INTERROGATIVE Tone Pattern 					 «EQ'N'EO'Q»
| marked by:
| - Rising/falling tone on any relevant possessive marker(s) (if existing) in the emphatic noun phrase¹
| - Rising/falling tone on the last syllable preceding the particle ‹mba›
| - Rising/falling tone on the particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'N'EO'Q_TR
        Associate a F tone at right edge of word.
        CONDITION:
           (
           (current morphname is «EQ'N'EO'Q»)
           )

| In the case of ‹nakêmbâ› and ‹khapêmbâ›:
\tone_rule EQ'N'EO'D_TR_a_TR (optional)
        Associate a F tone at right edge of word,
        Associate a F tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «EQ'N'EO'Q»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE DECLARATIVE Tone Pattern 												«EQ'N'D»
| marked by:
| - High tone on the last syllable preceding the particle ‹mba›
| - High tone on the particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'N'D_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (
           (current morphname is «EQ'N'D»)
           )

| In the case of ‹nakémbá› and ‹khapémbá›:
\tone_rule EQ'N'D_TR_a_TR (optional)
        Associate a H tone at right edge of word,
        Associate a H tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «EQ'N'D»)
           )

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE INTERROGATIVE Tone Pattern 												«EQ'N'Q»
| marked by:
| - Rising/falling tone on the last syllable preceding the particle ‹mba›
| - Rising/falling tone on the particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EQ'N'Q_TR
        Associate a F tone at right edge of word.
        CONDITION:
           (
           (current morphname is «EQ'N'Q»)
           )

| In the case of ‹nakêmbâ› and ‹khapêmbâ›:
\tone_rule EQ'N'D_TR_a_TR (optional)
        Associate a F tone at right edge of word,
        Associate a F tone at penultimate syllable of word.

        CONDITION:
           (
           (current morphname is «EQ'N'Q»)
           )
|]
|[ C2) ********** Attention Drawing Tone Patterns **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| ATTENTION DRAWING (Non-Verbal) DECLARATIVE Tone Pattern 								«ATTDR'NV'D»
| marked by:
| - High tone on the first syllable of the utterance
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule ATTDR'NV'D_TR
        Associate a H tone at left edge of word.

        CONDITION:
           (current morphname is «ATTDR'NV'D»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| ATTENTION DRAWING (Non-Verbal) INTERROGATIVE Tone Pattern 							«ATTDR'NV'Q»
| marked by:
| - Rising/falling tone on the last syllable of the utterance
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule ATTDR'NV'Q_TR
        Associate a F tone at left edge of word.

        CONDITION:
           (current morphname is «ATTDR'NV'Q»)
|]
|}
|{ D)  ########## TONE RULES FOR NON-VERBAL CLAUSES - COMPLEMENTATIONAL ############################

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| COMPLEMENTATIONAL EQUATIVE DECLARATIVE Tone Pattern 									 «CMPL"EQ'D»
| marked by:
| - Mid tone on any relevant possessive marker(s) (if existing)¹
| - Mid tone on the last syllable of the clause
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule CMPL"EQ'D_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (current morphname is «CMPL"EQ'D»)

|}
|{ E)  ########## TONE RULES FOR PARTICLES #########################################################

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EMPHATIC CONJUNCTION Tone 																	«EC»
| marked by:
| - Mid tone on the last syllable of the conjunction ‹neka›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule EC_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (current morphname is «EC»)


|}
|{ F)  ########## AUXILIARY TONES ##################################################################

| The tones treated here are peripheral tones belonging to some of the above tone patterns.
| TonePars marks these tones so that "SENT Disambig" can disambiguate tone patterns and (hopefully)
| pick the correct one among similar ones (e.g. «NINT'FV'EDF'Q» among all the Non-Intentive ones)

| E1) ********** POSSESSIVE AUXILIARY TONES **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| POSSESSIVE HIGH Tone «X'POSS'H» (Auxiliary)
| marked by:
| - High Tone on a possessive marker
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'POSS'H_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (current morphname is «X'POSS'H»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| POSSESSIVE MIDDLE Tone «X'POSS'M» (Auxiliary)
| marked by:
| - Middle Tone on a possessive marker
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'POSS'M_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (current morphname is «X'POSS'M»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| POSSESSIVE RISING/FALLING Tone «X'POSS'F» (Auxiliary)
| marked by:
| - Rising/Falling Tone on a possessive marker
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'POSS'F_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (current morphname is «X'POSS'F»)

| E2) ********** AUXILIARY TONES ON NPS/PARTICLES **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| HIGH Tone on NPs/Particles «X'NP'H» (Auxiliary)
| marked by:
| - High Tone on the last syllable of NPs/Particles
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'NP'H_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (current morphname is «X'NP'H»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| MIDDLE Tone on NPs/Particles «X'NP'M» (Auxiliary)
| marked by:
| - Middle Tone on the last syllable of NPs/Particles
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'NP'M_TR
        Associate a M tone at right edge of word.

        CONDITION:
           (current morphname is «X'NP'M»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| RISING/FALLING Tone on NPs/Particles «X'NP'F» (Auxiliary)
| marked by:
| - Rising/falling Tone on the last syllable of NPs/Particles
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'NP'F_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (current morphname is «X'NP'F»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| LOW Tone on NPs/Particles «X'NP'L» (Auxiliary)
| marked by:
| - Low Tone on the last syllable of NPs/Particles
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'NP'L_TR
        Associate a L tone at right edge of word.

        CONDITION:
           (current morphname is «X'NP'L»)

| E3) ********** DEMONSTRATIVE AUXILIARY TONES **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| DEMONSTRATIVE HIGH Tone «X'DEM'H» (Auxiliary)
| marked by:
| - High Tone on the first syllable on Emphasized Demonstratives
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'DEM'H_TR
        Associate a H tone at left edge of word.

        CONDITION:
           (current morphname is «X'DEM'H»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| DEMONSTRATIVE MIDDLE Tone «X'DEM'M» (Auxiliary)
| marked by:
| - Middle Tone on the first syllable on Emphasized Demonstratives
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'DEM'M_TR
        Associate a M tone at left edge of word.

        CONDITION:
           (current morphname is «X'DEM'M»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| DEMONSTRATIVE RISING/FALLING Tone «X'DEM'F» (Auxiliary)
| marked by:
| - Rising/falling Tone on the first syllable on Emphasized Demonstratives
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'DEM'F_TR
        Associate a F tone at left edge of word.

        CONDITION:
           (current morphname is «X'DEM'F»)

| E3) ********** EQUATIVE NEGATIVE AUXILIARY TONES **********
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE HIGH Tone «X'EQ'N'H» (Auxiliary)
| marked by:
| - High Tone on the last syllable on NPs preceding the negative particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'EQ'N'H_TR
        Associate a H tone at right edge of word.

        CONDITION:
           (current morphname is «X'EQ'N'H»)

|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
| EQUATIVE NEGATIVE RISING/FALLING Tone «X'EQ'N'F» (Auxiliary)
| marked by:
| - Rising/Falling Tone on the last syllable on NPs preceding the negative particle ‹mba›
|~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

\tone_rule X'EQ'N'F_TR
        Associate a F tone at right edge of word.

        CONDITION:
           (current morphname is «X'EQ'N'F»)

|}
|--------------------------------------------------
                        | post-lexical rules


|                       CALCULATE EDGE CONDITIONS
|***************************************************************************************************
|

|                       ORTHOGRAPHY CHANGES (\dch)
|***************************************************************************************************
|
\dch "-" ""


| Footnotes:
| ¹ tone marks on words preceding or following the verb/clause nucleus are disambiguated with
|   Sentrans (CarlaStudio: Analysis - Syntax - Surrounding Words Rules (KVGdisam.sen))
|   with the help of 'Auxiliary Tones' defined in the dictionary and in this file

| ² where absolutive verb prefixes are counted as belonging to the verb stem