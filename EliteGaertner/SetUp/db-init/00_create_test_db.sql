SELECT 'CREATE DATABASE elitegaertner_test'
    WHERE NOT EXISTS (
  SELECT FROM pg_database WHERE datname = 'elitegaertner_test'
)\gexec