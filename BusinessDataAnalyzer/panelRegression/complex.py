import os
import pandas as pd

from linearmodels import RandomEffects
import statsmodels.api as sm

from filters import filterOutLayers, filterCompaniesWithOneRecord

def baseTestOwnerEffect(df, foreignOwnerColumnName, ownershipConcentrationColumnName, performanceVariable):
    exog_vars = [foreignOwnerColumnName, "Vlastnik_PO", ownershipConcentrationColumnName, "Jednoosobova_SRO", "Statny_vlastnik", "Sekcia"]
    exog = sm.add_constant(df[exog_vars])
    endog = df[performanceVariable]

    # random effects model
    model_re = RandomEffects(endog, exog) 
    re_res = model_re.fit() 

    # print results
    print(re_res)

def main(performanceVariable):
    col_list = [
        "ICO",
        "Rok",
        "ROA",
        "ROE",
        "Zahranicny_vlastnik_FDI",
        "Zahranicny_vlastnik_Majoritny",
        "Vlastnik_PO",
        "Statny_vlastnik",
        "Koncentracia_vlastnictva_h3",
        "Koncentracia_vlastnictva_h5",
        "Koncentracia_vlastnictva_t3",
        "Koncentracia_vlastnictva_t5",
        "Jednoosobova_SRO",
        "Sekcia"
    ]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/panel_data.csv"), usecols=col_list, delimiter=';', encoding='utf8', index_col=["ICO", "Rok"])

    dfWithoutOutLayer = filterOutLayers(df, performanceVariable)
    dfCleared = filterCompaniesWithOneRecord(dfWithoutOutLayer)

    print("\n Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h3 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h3", performanceVariable)
    
    print("\n Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h5 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h5", performanceVariable)

    print("\n Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t3 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t3", performanceVariable)

    print("\n Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t5 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t5", performanceVariable)


    print("\n Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_h3 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_h3", performanceVariable)
    
    print("\n Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_h5 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_h5", performanceVariable)

    print("\n Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_t3 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_t3", performanceVariable)

    print("\n Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_t5 \n")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_t5", performanceVariable)

if __name__ == "__main__":
    print("\n \n \n \n \n")
    print("ROE")
    print("\n \n \n \n \n")
    main("ROE")
    
    print("\n \n \n \n \n")
    print("ROA")
    print("\n \n \n \n \n")
    main("ROA")
