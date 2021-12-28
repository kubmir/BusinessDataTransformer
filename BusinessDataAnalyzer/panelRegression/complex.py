import os
import pandas as pd

from linearmodels import PanelOLS
from linearmodels import RandomEffects
import statsmodels.api as sm

import numpy.linalg as la
from scipy import stats
import numpy as np

from sklearn.preprocessing import StandardScaler
from boxplotFilter import filterOutLayers

# def filterOutLayers(df, currentAnalyzedCol):
#     if currentAnalyzedCol == "ROA":
#         return df[(df.ROA > -3) & (df.ROA < 3) & (df.ROA != 0)]
#     else:
#         return df[(df.ROE > -3) & (df.ROE < 3) & (df.ROE != 0)]

def filterCompaniesWithOneRecord(df):
    df_count = df.reset_index().groupby("ICO").agg({"ROA":"count", "ROE":"count"}).reset_index()
    ICO_all_years = df_count.loc[(df_count.ROA > 1) & (df_count.ROE > 1)]["ICO"]
    dff = df.reset_index()
    return dff.loc[dff.ICO.isin(ICO_all_years)].set_index(["ICO", "Rok"])
    

def baseTestOwnerEffect(performance_variable):
    col_list = [
        "ICO",
        "Rok",
        "ROA",
        "ROE",
        "Zahranicny_vlastnik",
        "Institucionalny_vlastnik",
        "Koncentracia_vlastnictva",
        "Jednoosobova_SRO",
        "Sekcia",
        "NACE",
        "NACE_2"
    ]

    df = pd.read_csv("./panel_data_nace.csv", usecols=col_list, delimiter=';', encoding='utf8', index_col=["ICO", "Rok"])

    dfWithoutOutLayer = filterOutLayers(df, performance_variable)
    dfCleared = filterCompaniesWithOneRecord(dfWithoutOutLayer)

    exog_vars = ["Zahranicny_vlastnik", "Institucionalny_vlastnik", "Koncentracia_vlastnictva", "Jednoosobova_SRO", "Sekcia"]
    exog = sm.add_constant(dfCleared[exog_vars])
    endog = dfCleared[performance_variable]

    # random effects model
    model_re = RandomEffects(endog, exog) 
    re_res = model_re.fit() 

    # fixed effects model
    model_fe = PanelOLS(endog, exog, entity_effects = True, drop_absorbed=True) 
    fe_res = model_fe.fit() 

    #print results
    print(re_res)
    print(fe_res)

    # Hausman test
    hausman_results = hausman(fe_res, re_res) 
    
    print("chi-Squared: {}".format(str(hausman_results[0])))
    print("degrees of freedom: {}".format(str(hausman_results[1])))
    print("p-Value: {}".format(str(hausman_results[2])))
    
    return df

def hausman(fe, re):
    b = fe.params
    B = re.params
    v_b = fe.cov
    v_B = re.cov

    df = b[np.abs(b) < 1e8].size
    chi2 = np.dot((b - B).T, la.inv(v_b - v_B).dot(b - B))
    pval = stats.chi2.sf(chi2, df)
    
    return chi2, df, pval

def main():
    baseTestOwnerEffect("ROE")
    # TODO: standardne odchylky - autokorelacia check - robustne standardne chyby, clustrovanie ?, kovariancna matica
    # normalizovat koncentraciu napr. /10 000 - esteticka vyhoda
    # casove vplyvy a odvetvia
    # napr. priemerne hodnoty, delit zap. / klad. roe
    # cyklus - priemer rokov, casove vplyvy - pozor na zmeny
if __name__ == "__main__":
    main()