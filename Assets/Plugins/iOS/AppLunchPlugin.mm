extern "C"
{
    void lunchApp(const char* bookURLs){
        //reference BOOK URL is here
        //NSURL *url = [NSURL URLWithString:@"itms-books://jhkhkhk"];
        //NSURL *url = [NSURL URLWithString:@"itms-books://"+ bookURLs ];
        NSLog(@"book url = %@", [NSString stringWithUTF8String: bookURLs]);
        NSString *urlstring = [NSString stringWithFormat:@"itms-books://%@" , [NSString stringWithUTF8String: bookURLs]];
        NSURL *url = [NSURL URLWithString:urlstring];
        NSLog(@"url = %@", url);
              //check URL can open or not
              if([[UIApplication sharedApplication] canOpenURL:url ]){
                  [[UIApplication sharedApplication] openURL:url ];
              }
              else{
                  [[[UIAlertView alloc] initWithTitle:@"iBooks"
                                              message:NSLocalizedString(@"Book URL is something wrong!!!", nil)
                                             delegate:nil
                                    cancelButtonTitle:@"OK"
                                    otherButtonTitles:nil, nil] show];
              }
              
    }
    
    NSString* CreateNSString(const char* stringurl)
    {
        if (stringurl)
            return [NSString stringWithUTF8String: stringurl];
        else
            return [NSString stringWithUTF8String: ""];
    }
              
}
