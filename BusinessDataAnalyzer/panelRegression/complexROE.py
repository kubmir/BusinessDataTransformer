import os
import pandas as pd

from linearmodels import RandomEffects
import statsmodels.api as sm

from filters import filterOutLayers, filterCompaniesWithOneRecord

def baseTestOwnerEffect(df, foreignOwnerColumnName, ownershipConcentrationColumnName):
    exog_vars = [foreignOwnerColumnName, "Vlastnik_PO", ownershipConcentrationColumnName, "Jednoosobova_SRO", "Sekcia"]
    exog = sm.add_constant(df[exog_vars])
    endog = df["ROE"]

    # random effects model
    model_re = RandomEffects(endog, exog) 
    re_res = model_re.fit() 

    # print results
    print(re_res)

def main():
    col_list = [
        "ICO",
        "Rok",
        "ROA",
        "ROE",
        "Zahranicny_vlastnik_FDI",
        "Zahranicny_vlastnik_Majoritny",
        "Vlastnik_PO",
        "Koncentracia_vlastnictva_h3",
        "Koncentracia_vlastnictva_h5",
        "Koncentracia_vlastnictva_t3",
        "Koncentracia_vlastnictva_t5",
        "Jednoosobova_SRO",
        "Sekcia"
    ]
    df = pd.read_csv(os.path.expanduser("~/Desktop/Diplomovka_ESF/panel_data.csv"), usecols=col_list, delimiter=';', encoding='utf8', index_col=["ICO", "Rok"])

    dfWithoutOutLayer = filterOutLayers(df, "ROE")
    dfCleared = filterCompaniesWithOneRecord(dfWithoutOutLayer)

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h3")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h3")
    
    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h5")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h5")

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t3")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t3")

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t5")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t5")

    print("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")

    print("Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_h3")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_h3")
    
    print("Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_h5")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_h5")

    print("Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_t3")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_t3")

    print("Zahranicny_vlastnik_Majoritny + Koncentracia_vlastnictva_t5")
    baseTestOwnerEffect(dfCleared, "Zahranicny_vlastnik_Majoritny", "Koncentracia_vlastnictva_t5")

if __name__ == "__main__":
    main()