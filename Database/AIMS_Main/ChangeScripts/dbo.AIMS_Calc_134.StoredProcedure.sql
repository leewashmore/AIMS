S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O F F  
 G O  
 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
 - -   P u r p o s e : 	 T h i s   p r o c e d u r e   c a l c u l a t e s   t h e   v a l u e   f o r   D A T A _ I D : 1 3 4   Y i e l d    
 - - 	 	 	 o n   I n t e r e s t   E a r n i n g   A s s e t s  
 - -  
 - - 	 	 	 ( S I I B   f o r   Y e a r /   A V E R A G E ( S O E A + A N T L + A S E C   f o r   Y e a r ,   S O E A + A N T L + A S E C   f o r   P r i o r   Y e a r ) ) - 1  
 - -  
 - -   A u t h o r : 	 D a v i d   M u e n c h  
 - -   D a t e : 	 J u l y   2 ,   2 0 1 2  
 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
 a l t e r   p r o c e d u r e   [ d b o ] . [ A I M S _ C a l c _ 1 3 4 ] (  
 	 @ I S S U E R _ I D 	 	 	 v a r c h a r ( 2 0 )   =   N U L L 	 	 	 - -   T h e   c o m p a n y   i d e n t i f i e r 	 	  
 , 	 @ C A L C _ L O G 	 	 	 c h a r 	 	 =   ' Y ' 	 	 	 - -   W r i t e   e r r o r s   t o   t h e   C A L C _ L O G   t a b l e .  
 )  
 a s  
  
 	 - -   G e t   t h e   d a t a  
 	 s e l e c t   p f . *    
 	     i n t o   # A  
 	     f r o m   d b o . P E R I O D _ F I N A N C I A L S   p f     w i t h   ( n o l o c k )  
 	   w h e r e   D A T A _ I D   =   9   	 	 	 	 	 - -   I n t e r e s t   I m c o m e ,   B a n k   ( S I I B )  
 	       a n d   p f . I S S U E R _ I D   =   @ I S S U E R _ I D  
 	       a n d   p f . P E R I O D _ T Y P E   =   ' A '  
  
 	 s e l e c t   p f . *    
 	     i n t o   # B  
 	     f r o m   d b o . P E R I O D _ F I N A N C I A L S   p f     w i t h   ( n o l o c k )  
 	   w h e r e   D A T A _ I D   =   2 9 2 	 	 	 	 	 - -   I n t e r e s t   E a r n i n g   A s s e t s  
 	       a n d   p f . I S S U E R _ I D   =   @ I S S U E R _ I D  
 	       a n d   p f . P E R I O D _ T Y P E   =   ' A '  
  
 	 - -   A d d   t h e   d a t a   t o   t h e   t a b l e  
 	 B E G I N   T R A N   T 1  
 	 i n s e r t   i n t o   P E R I O D _ F I N A N C I A L S ( I S S U E R _ I D ,   S E C U R I T Y _ I D ,   C O A _ T Y P E ,   D A T A _ S O U R C E ,   R O O T _ S O U R C E  
 	 	     ,   R O O T _ S O U R C E _ D A T E ,   P E R I O D _ T Y P E ,   P E R I O D _ Y E A R ,   P E R I O D _ E N D _ D A T E ,   F I S C A L _ T Y P E ,   C U R R E N C Y  
 	 	     ,   D A T A _ I D ,   A M O U N T ,   C A L C U L A T I O N _ D I A G R A M ,   S O U R C E _ C U R R E N C Y ,   A M O U N T _ T Y P E )  
 	 s e l e c t   a . I S S U E R _ I D ,   a . S E C U R I T Y _ I D ,   a . C O A _ T Y P E ,   a . D A T A _ S O U R C E ,   a . R O O T _ S O U R C E  
 	 	 ,     a . R O O T _ S O U R C E _ D A T E ,   a . P E R I O D _ T Y P E ,   a . P E R I O D _ Y E A R ,   a . P E R I O D _ E N D _ D A T E  
 	 	 ,     a . F I S C A L _ T Y P E ,   a . C U R R E N C Y  
 	 	 ,     1 3 4   a s   D A T A _ I D 	 	 	 	 	 	 	 	 	 	 - -   D A T A _ I D : 1 3 4   Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s  
 	 	 ,     C A S E   W H E N   a . A M O U N T   > =   0   a n d   ( b . A M O U N T + c . A M O U N T ) >   0     T H E N   a . A M O U N T   /   ( ( b . A M O U N T + c . A M O U N T ) / 2 )  
 	 	 	 	 E L S E   N U L L    
 	 	 	 	 E N D   a s   A M O U N T 	 	 - -   I n t e r e s t   I n c o m e ,   B a n k   /   A v g ( 2 9 2 + 2 9 2 ) / 2  
 	 	 ,     ' I n t e r e s t   I n c o m e ( '   +   C A S T ( a . A M O U N T   a s   v a r c h a r ( 3 2 ) )   +   ' )   /   (   # 2 9 2 ( '   +   C A S T ( b . A M O U N T   a s   v a r c h a r ( 3 2 ) )   +   ' ) +   # 2 9 2   p r i o r   y e a r ( '   +   C A S T ( c . A M O U N T   a s   v a r c h a r ( 3 2 ) )   +   ' ) ) / 2 '   a s   C A L C U L A T I O N _ D I A G R A M  
 	 	 ,     a . S O U R C E _ C U R R E N C Y  
 	 	 ,     a . A M O U N T _ T Y P E  
 	     f r o m   # A   a  
 	   i n n e r   j o i n 	 # B   b   o n   b . I S S U E R _ I D   =   a . I S S U E R _ I D    
 	 	 	 	 	 a n d   b . D A T A _ S O U R C E   =   a . D A T A _ S O U R C E   a n d   b . P E R I O D _ T Y P E   =   a . P E R I O D _ T Y P E  
 	 	 	 	 	 a n d   b . P E R I O D _ Y E A R   =   a . P E R I O D _ Y E A R   a n d   b . F I S C A L _ T Y P E   =   a . F I S C A L _ T Y P E  
 	 	 	 	 	 a n d   b . C U R R E N C Y   =   a . C U R R E N C Y  
 	   i n n e r   j o i n 	 # B   c   o n   c . I S S U E R _ I D   =   a . I S S U E R _ I D    
 	 	 	 	 	 a n d   c . D A T A _ S O U R C E   =   a . D A T A _ S O U R C E   a n d   c . P E R I O D _ T Y P E   =   a . P E R I O D _ T Y P E  
 	 	 	 	 	 a n d   c . P E R I O D _ Y E A R   =   a . P E R I O D _ Y E A R - 1   a n d   c . F I S C A L _ T Y P E   =   a . F I S C A L _ T Y P E  
 	 	 	 	 	 a n d   c . C U R R E N C Y   =   a . C U R R E N C Y  
 	   w h e r e   1 = 1    
 	     a n d   i s n u l l ( a . A M O U N T , 0 . 0 )   > = 0 . 0   a n d   ( i s n u l l ( b . A M O U N T ,   0 . 0 ) + i s n u l l ( c . A M O U N T , 0 . 0 ) )   >   0 . 0 	 - -   D a t a   v a l i d a t i o n  
 - - 	   o r d e r   b y   a . I S S U E R _ I D ,   a . C O A _ T Y P E ,   a . D A T A _ S O U R C E ,   a . P E R I O D _ T Y P E ,   a . P E R I O D _ Y E A R ,     a . F I S C A L _ T Y P E ,   a . C U R R E N C Y  
 	 C O M M I T   T R A N   T 1  
  
 	 i f   @ C A L C _ L O G   =   ' Y '  
 	 	 B E G I N  
 	 	 	 - -   E r r o r   c o n d i t i o n s   -   N U L L   o r   Z e r o   d a t a    
 	 	 	 i n s e r t   i n t o   C A L C _ L O G (   L O G _ D A T E ,   D A T A _ I D ,   I S S U E R _ I D ,   P E R I O D _ T Y P E ,   P E R I O D _ Y E A R  
 	 	 	 	 	 	 	 	 ,   P E R I O D _ E N D _ D A T E ,   F I S C A L _ T Y P E ,   C U R R E N C Y ,   T X T )  
 	 	 	 (  
 	 	 	 s e l e c t   G E T D A T E ( )   a s   L O G _ D A T E ,   1 3 4   a s   D A T A _ I D ,   a . I S S U E R _ I D ,   a . P E R I O D _ T Y P E  
 	 	 	 	 ,     a . P E R I O D _ Y E A R ,   a . P E R I O D _ E N D _ D A T E ,   a . F I S C A L _ T Y P E ,   a . C U R R E N C Y  
 	 	 	 	 ,   ' E R R O R   c a l c u l a t i n g   1 3 4 : Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s .     D A T A _ I D : 2 9 2   i s   N U L L   o r   Z E R O '   a s   T X T  
 	 	 	     f r o m   # B   a  
 	 	 	   w h e r e   i s n u l l ( a . A M O U N T ,   0 . 0 )   =   0 . 0 	 - -   D a t a   e r r o r  
 	 	 	 )   u n i o n   (  
 	 	 	  
 	 	 	 - -   E r r o r   c o n d i t i o n s   -   m i s s i n g   d a t a    
 	 	 	 s e l e c t   G E T D A T E ( )   a s   L O G _ D A T E ,   1 3 4   a s   D A T A _ I D ,   a . I S S U E R _ I D ,   a . P E R I O D _ T Y P E  
 	 	 	 	 ,     a . P E R I O D _ Y E A R ,   a . P E R I O D _ E N D _ D A T E ,   a . F I S C A L _ T Y P E ,   a . C U R R E N C Y  
 	 	 	 	 ,   ' E R R O R   c a l c u l a t i n g   1 3 4 : Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s .     D A T A _ I D : 2 9 2   i s   m i s s i n g '   a s   T X T  
 	 	 	     f r o m   # A   a  
 	 	 	     l e f t   j o i n 	 # B   b   o n   b . I S S U E R _ I D   =   a . I S S U E R _ I D    
 	 	 	 	 	 	 	 a n d   b . D A T A _ S O U R C E   =   a . D A T A _ S O U R C E   a n d   b . P E R I O D _ T Y P E   =   a . P E R I O D _ T Y P E  
 	 	 	 	 	 	 	 a n d   b . P E R I O D _ Y E A R   =   a . P E R I O D _ Y E A R   a n d   b . F I S C A L _ T Y P E   =   a . F I S C A L _ T Y P E  
 	 	 	 	 	 	 	 a n d   b . C U R R E N C Y   =   a . C U R R E N C Y  
 	 	 	   w h e r e   1 = 1   a n d   b . I S S U E R _ I D   i s   N U L L  
 	 	 	 )   u n i o n   (  
  
 	 	 	 - -   E r r o r   c o n d i t i o n s   -   m i s s i n g   d a t a    
 	 	 	 s e l e c t   G E T D A T E ( )   a s   L O G _ D A T E ,   1 3 4   a s   D A T A _ I D ,   a . I S S U E R _ I D ,   a . P E R I O D _ T Y P E  
 	 	 	 	 ,     a . P E R I O D _ Y E A R ,     a . P E R I O D _ E N D _ D A T E ,     a . F I S C A L _ T Y P E ,     a . C U R R E N C Y  
 	 	 	 	 ,   ' E R R O R   c a l c u l a t i n g   1 3 4 : Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s .     D A T A _ I D : 9   S I I B   i s   m i s s i n g '  
 	 	 	     f r o m   # B   a  
 	 	 	     l e f t   j o i n 	 # A   b   o n   b . I S S U E R _ I D   =   a . I S S U E R _ I D    
 	 	 	 	 	 	 	 a n d   b . D A T A _ S O U R C E   =   a . D A T A _ S O U R C E   a n d   b . P E R I O D _ T Y P E   =   a . P E R I O D _ T Y P E  
 	 	 	 	 	 	 	 a n d   b . P E R I O D _ Y E A R   =   a . P E R I O D _ Y E A R   a n d   b . F I S C A L _ T Y P E   =   a . F I S C A L _ T Y P E  
 	 	 	 	 	 	 	 a n d   b . C U R R E N C Y   =   a . C U R R E N C Y  
 	 	 	   w h e r e   1 = 1   a n d   b . I S S U E R _ I D   i s   N U L L  
 	 	 	 )   u n i o n   (  
  
 	 	 	 - -   E R R O R   -   N o   d a t a   a t   a l l   a v a i l a b l e  
 	 	 	 s e l e c t   G E T D A T E ( )   a s   L O G _ D A T E ,   1 3 4   a s   D A T A _ I D ,   i s n u l l ( @ I S S U E R _ I D ,   '   ' )   a s   I S S U E R _ I D ,   '   '   a s   P E R I O D _ T Y P E  
 	 	 	 	 ,     0   a s   P E R I O D _ Y E A R ,     ' 1 / 1 / 1 9 0 0 '   a s   P E R I O D _ E N D _ D A T E ,     '   '   a s   F I S C A L _ T Y P E ,     '   '   a s   C U R R E N C Y  
 	 	 	 	 ,   ' E R R O R   c a l c u l a t i n g   1 3 4 : Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s .     D A T A _ I D : 2 9 2   n o   d a t a '   a s   T X T  
 	 	 	     f r o m   ( s e l e c t   C O U N T ( * )   C N T   f r o m   # B   h a v i n g   C O U N T ( * )   =   0 )   z  
 	 	 	 )   u n i o n   (  
 	 	 	 s e l e c t   G E T D A T E ( )   a s   L O G _ D A T E ,   1 3 4   a s   D A T A _ I D ,   i s n u l l ( @ I S S U E R _ I D ,   '   ' )   a s   I S S U E R _ I D ,   '   '   a s   P E R I O D _ T Y P E  
 	 	 	 	 ,     0   a s   P E R I O D _ Y E A R ,     ' 1 / 1 / 1 9 0 0 '   a s   P E R I O D _ E N D _ D A T E ,     '   '   a s   F I S C A L _ T Y P E ,     '   '   a s   C U R R E N C Y  
 	 	 	 	 ,   ' E R R O R   c a l c u l a t i n g   1 3 4 : Y i e l d   o n   I n t e r e s t   E a r n i n g   A s s e t s .     D A T A _ I D : 9   S I I B   n o   d a t a '   a s   T X T  
 	 	 	     f r o m   ( s e l e c t   C O U N T ( * )   C N T   f r o m   # A   h a v i n g   C O U N T ( * )   =   0 )   z  
 	 	 	 )  
 	 	 E N D  
 	 	  
 	 d r o p   t a b l e   # A  
 	 d r o p   t a b l e   # B  
 G O  
 