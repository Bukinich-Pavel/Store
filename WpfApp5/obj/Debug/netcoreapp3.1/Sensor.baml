   M S B A M L   `   `   `  ����@  <Store, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp5WpfApp5  5   	       WpfApp5.Sensor   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp5     5   	     � Title$  Sensor��5   	   $	��450��6   $	��800��6%    � Icon$ 	/logo.png��61   .��5
      � -   5         �Name$ 
GridSensor��5
      ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FFC3C2C2��6    � Offset$ 0Q�61   � 5      $ White��6   $ 1Q�6-   5       � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5   
   �� -   5         ��Name$ Photo��5      $	��500��5   .   $	��500��6=   ��6   �� 	 �� Path$	 photo�6=   �� 5   
   $��1��6   $
��Left=�6%   $	��Top=�6@   $��
20,40,20,0q�6X   $	��350��6l   $	��200��6{   .+�5      �� -   5        
 ��Name$

 Name��5      $��24(�6@   ��6&   �� $		 txt�6@   �� 5      -   5        $

 Volt��5      $��24(�6A   ��6&   �� $
	 volt�6A   �� 5      -   5        $

 Size��5      $��24(�6A   ��6&   �� $
	 size�6A   �� 5      -   5        $
 	Connector��5      $��24(�6I   ��6+   �� $	 connect�6I   ]� 5      $��False��6   $��0,20,0,0q�6.   .+�5       �� -   5         ��Name$
 Less��5       $��-��6'   $��20��63   $��24(�6A   6O   �� 5!      -   5        $
 	Existence��5!      $��24(�6H   ��6/   �� $	 ex�6H   �� 5"      -	   5        $
 More��5"      $��+��6'   $��20��63   $��24(�6A   6O   5#      5$      5&      5'      