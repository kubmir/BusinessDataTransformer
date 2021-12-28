import os
import pandas as pd

from linearmodels import PanelOLS
from linearmodels import RandomEffects
import statsmodels.api as sm

from scipy import stats
import numpy as np
import numpy.linalg as la

from filters import filterOutLayers, filterCompaniesWithOneRecord

def baseTestOwnerEffect(df, foreignOwnerColumnName, ownershipConcentrationColumnName, performanceVariable):
    exog_vars = [foreignOwnerColumnName, "Vlastnik_PO", ownershipConcentrationColumnName, "Jednoosobova_SRO"]
    exog = sm.add_constant(df[exog_vars])
    endog = df[performanceVariable]

    # random effects model
    model_re = RandomEffects(endog, exog, check_rank=False) 
    re_res = model_re.fit() 

    # fixed effects model
    model_fe = PanelOLS(endog, exog, entity_effects = True, time_effects=True) 
    fe_res = model_fe.fit() 

    hausman_results = hausman(fe_res, re_res)

    print("Hausmann p-Value: {}".format(str(hausman_results[2])))

    if hausman_results[2] < 0.05:
        print(fe_res)
    else:
        print(re_res)


def filterSectionCompanies(df, currentSection):
    return df[df["Sekcia"] == currentSection]

def hausman(fe, re):
    b = fe.params
    B = re.params
    v_b = fe.cov
    v_B = re.cov

    df = b[np.abs(b) < 1e8].size
    chi2 = np.dot((b - B).T, la.inv(v_b - v_B).dot(b - B))
    pval = stats.chi2.sf(chi2, df)
    
    return chi2, df, pval

def main(performanceVariable, sectionValue):
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

    dfWithoutOutLayer = filterOutLayers(df, performanceVariable)
    dfCleared = filterCompaniesWithOneRecord(dfWithoutOutLayer)
    sectionCompanies = filterSectionCompanies(dfCleared, sectionValue)
    
    #print("\n Dataframe rows", sectionCompanies.size, " for section", sectionValue, "\n")

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h3")
    baseTestOwnerEffect(sectionCompanies, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h3", performanceVariable)
    
    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_h5")
    baseTestOwnerEffect(sectionCompanies, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_h5", performanceVariable)

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t3")
    baseTestOwnerEffect(sectionCompanies, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t3", performanceVariable)

    print("Zahranicny_vlastnik_FDI + Koncentracia_vlastnictva_t5")
    baseTestOwnerEffect(sectionCompanies, "Zahranicny_vlastnik_FDI", "Koncentracia_vlastnictva_t5", performanceVariable)

if __name__ == "__main__":
    sections = [
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        # "O", mame udaje iba o 4 firmach co nie je postacujuce a vela stlpcov nema full-rank
        "P",
        "Q",
        "R",
        "S",
        # "T", no company data
        # "U", no company data
    ]

    for section in sections:
        print("\n                                         Analyzing section", section, "\n")

        print("\nROE\n")
        main("ROE", section)

        print("\ROA\n")
        main("ROA", section)